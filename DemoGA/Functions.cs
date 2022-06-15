using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Data;

namespace DemoGA
{
    public static class Functions
    {
        // Tạo TKB
        public static List<Timetable> GetInput(int n_pop, ClassInfo classInfo, List<TeacherInfo> teachers, List<SubjectInfo> subjects, List<TeachingDistribution> teachingDistributions, string section)
        {
            List<Timetable> result = new List<Timetable>();

            // Get samples
            Lessons[,] lessons = InitData.GetInputLessons(subjects, teachers, teachingDistributions);

            for (int i = 0; i < n_pop; i++)
            {
                Timetable t = new Timetable(classInfo);
                t.Lessons = InitData.Shuffle(new Random(), lessons);
                t.Section = section;

                AssignFixedLessons(subjects, ref t);

                result.Add(t);
            }

            return result;
        }

        // Xếp lại các tiết cố định sau khi TKB được xáo trộn
        public static void AssignFixedLessons(List<SubjectInfo> subjects, ref Timetable timetable)
        {
            // Assign fixed subjects
            var subjectHFL = subjects.Where(x => x.FixedLessons.Count > 0).ToList();

            // Find assigned fixed subjects address in timetable
            for (int row = 0; row < timetable.Lessons.GetLength(0); row++)
            {
                for (int column = 0; column < timetable.Lessons.GetLength(1); column++)
                {
                    var currentLesson = timetable.Lessons[row, column];

                    if (currentLesson == null) continue;

                    foreach (var s in subjectHFL)
                    {
                        if (currentLesson.Subject.Id == s.Id)
                        {
                            foreach (var address in s.FixedLessons)
                            {
                                var r = Convert.ToInt32(address.Split('_')[0]);
                                var c = Convert.ToInt32(address.Split('_')[1]);

                                var tmpLesson = new Lessons();
                                tmpLesson = currentLesson;

                                timetable.Lessons[row, column] = timetable.Lessons[r, c];
                                timetable.Lessons[r, c] = tmpLesson;
                            }
                        }
                    }
                }
            }
        }

        public static int EvaluationFitness(ref Timetable sample, List<SubjectInfo> subjects, List<TeacherAssignedLessonsInfo> teacherAssignedLessons)
        {
            int score = 0;

            List<MaximumLessons> trackingML = new List<MaximumLessons>();
            Lessons[,] lessons = sample.Lessons;
            int classId = sample.ClassInfo.Id;
            string section = sample.Section;

            // Loop through row (first dimenson) => Thứ
            for (int row = 0; row < lessons.GetLength(0); row++)
            {
                int currentLessonId = 0;
                int currentLessonCount = 0;

                // Loop through column (second dimension) => Tiết
                for (int column = 0; column < lessons.GetLength(1); column++)
                {
                    if (lessons[row, column] == null) continue;

                    string address = row.ToString() + "_" + column.ToString();

                    // RULE 1. Not duplicate lessons same teacher
                    var td = teacherAssignedLessons.Find(x => x.TeacherId == lessons[row, column].Teacher.Id);

                    if (td != null)
                    {
                        var info = td.AssignedLessonInfos.Find(x => x.LessonAddress == address && x.ClassId != classId && x.Section == section);

                        if (info != null && lessons[row, column].IsLock == 0)
                        {
                            score--;
                            var e = new TrackingError();
                            e.ClassName = sample.ClassInfo.Name;
                            e.Address = address;
                            e.ErrorType = 1;
                            e.Reason = String.Format("Trùng tiết của gv {0} môn {1} lớp {2} buổi {3}", lessons[row, column].Teacher.Name, lessons[row, column].Subject.Name, info.ClassName, section);

                            sample.Err.Add(e);
                        }
                    }

                    // RULE 2. Check maximum continous lessons
                    if (currentLessonId == lessons[row, column].Subject.Id) currentLessonCount++;
                    else
                    {
                        currentLessonId = lessons[row, column].Subject.Id;
                        currentLessonCount = 1;
                    }

                    if (column == 0) currentLessonCount = 1;

                    if (currentLessonCount > lessons[row, column].Subject.MaximumContinousLessons)
                    {
                        score--;
                        var e = new TrackingError();
                        e.ClassName = sample.ClassInfo.Name;
                        e.Address = address;
                        e.ErrorType = 2;
                        e.Reason = String.Format("Vượt quá số tiết liên tiếp môn {0}. Tổng số tiết {1}", lessons[row, column].Subject.Name, currentLessonCount);

                        sample.Err.Add(e);
                    }

                    // Count number of lesson
                    int index = trackingML.FindIndex(x => x.SubjectId == currentLessonId);

                    if (index < 0)
                        trackingML.Add(new MaximumLessons(lessons[row, column].Subject.Id, 1));
                    else
                        trackingML[index].CurentLessons++;
                }
            }

            // RULE 3. Lessons per week
            CheckLessonsPerWeekRule(ref score, ref sample, subjects, trackingML);

            return score;
        }

        // Kiểm tra số lượng tiết / tuần của môn có đủ hay không
        private static void CheckLessonsPerWeekRule(ref int score, ref Timetable sample, List<SubjectInfo> subjects, List<MaximumLessons> trackingML)
        {
            for (int i = 0; i < subjects.Count; i++)
            {
                var subjectId = subjects[i].Id;
                var tmp = trackingML.Find(x => x.SubjectId == subjectId);

                if (tmp == null)
                {
                    score--;
                    var e = new TrackingError();
                    e.ClassName = sample.ClassInfo.Name;
                    e.ErrorType = 3;
                    e.Reason = String.Format("Thiếu môn {0}", subjects[i].Id);

                    sample.Err.Add(e);
                }
                else if (tmp.CurentLessons != subjects[i].LessonsPerWeek)
                {
                    score--;
                    var e = new TrackingError();
                    e.ClassName = sample.ClassInfo.Name;
                    e.ErrorType = 4;
                    e.Reason = String.Format("Không đủ số tiết 1 tuần môn {0}", subjects[i].Id);

                    sample.Err.Add(e);
                }
            }
        }

        // Chọn ra TKB có điểm cao nhất
        public static Timetable Selection(List<Timetable> inputSamples, List<SubjectInfo> subjects, List<TeacherAssignedLessonsInfo> teacherAssignedLessonsInfos)
        {
            for (int i = 0; i < inputSamples.Count; i++)
            {
                var tmp = inputSamples[i];
                inputSamples[i].Score = EvaluationFitness(ref tmp, subjects, teacherAssignedLessonsInfos);

                inputSamples[i] = tmp;
            }

            var qry = from p in inputSamples
                      orderby p.Score descending
                      select p;

            var temp = qry.ToArray();

            return temp[0];
        }

        // Phối giống
        public static List<Timetable> Crossover(Timetable p1, Timetable p2, double crossRate)
        {
            Timetable c1 = new Timetable();
            Timetable c2 = new Timetable();

            c1.ClassInfo = c2.ClassInfo = p1.ClassInfo;

            // Check for recombination
            Random rnd = new Random();
            if (rnd.NextDouble() < crossRate)
            {
                int pt = rnd.Next(1, p1.Lessons.Length - 2);

                for (int i = 0; i < p1.Lessons.GetLength(0); i++)
                {
                    for (int j = 0; j < p1.Lessons.GetLength(1); j++)
                    {
                        // Nếu tiết đang xét là tiết cố định => gán cho TKB con tiết đó
                        if (p1.Lessons[i, j] != null && p1.Lessons[i, j].IsLock == 1)
                        {
                            c1.Lessons[i, j] = p1.Lessons[i, j];
                            c2.Lessons[i, j] = p2.Lessons[i, j];
                            continue;
                        }

                        if (i < pt)
                        {
                            c1.Lessons[i, j] = p1.Lessons[i, j];
                            c2.Lessons[i, j] = p2.Lessons[i, j];
                        }
                        else
                        {
                            c1.Lessons[i, j] = p2.Lessons[i, j];
                            c2.Lessons[i, j] = p1.Lessons[i, j];
                        }
                    }
                }
            }

            List<Timetable> result = new List<Timetable>();

            result.AddRange(new List<Timetable> { c1, c2 });

            return result;
        }

        // Đột biến => swap tiết 2 ô với nhau, tỉ lệ đột biến nhỏ
        public static void Mutation(ref Timetable sample, double mutationRate)
        {
            for (int i = 1; i < sample.Lessons.Length; i++)
            {
                Random rnd = new Random();
                double rndDouble = rnd.NextDouble();

                // Check xem random có nhỏ hơn tỉ lệ đột biến không, nếu nhỏ hơn => xảy ra đột biến
                if (rndDouble < mutationRate)
                {
                    for (int row = 0; row < sample.Lessons.GetLength(0); row++)
                    {
                        for (int col = 1; col < sample.Lessons.GetLength(1); col++)
                        {
                            var temp = sample.Lessons[row, col];
                            var prev = sample.Lessons[row, col - 1];

                            // Nếu tiết đang xét là tiết cố định => skip
                            if ((temp != null && temp.IsLock == 1) || (prev != null && prev.IsLock == 1)) continue;

                            sample.Lessons[row, col] = prev;
                            sample.Lessons[row, col - 1] = temp;
                        }
                    }
                }
            }
        }

        public static void GeneticAlgorithm2(int n_iter, int n_pop, double r_cross, double r_mut, ref Timetable timeTable, ref List<TeacherAssignedLessonsInfo> teacherAssignedLessons)
        {
            // Get input sample list
            int classId = timeTable.ClassInfo.Id;
            List<TeacherInfo> teachers = InitData.GetListTeacher();
            List<TeachingDistribution> teachingDistributions = InitData.GetTeachingDistributions();
            List<TeachingDistribution> td = teachingDistributions.Where(x => x.ClassessId.Any(y => y == classId)).ToList();

            string kindOfSection = timeTable.Section == timeTable.ClassInfo.MainSection ? InitData.PRIMARY_SECTION : InitData.SECONDARY_SECTION;
            List<SubjectInfo> subjects = timeTable.ClassInfo.Subjects.Where(x => x.Section == kindOfSection).ToList();

            List<TimetableContainer> sampleContainer = new List<TimetableContainer>();

            for (int i = 0; i < n_pop; i++)
            {
                List<Timetable> input = GetInput(n_pop, timeTable.ClassInfo, teachers, subjects, td, timeTable.Section);

                sampleContainer.Add(new TimetableContainer(input));
            }

            // Giả sử TKB đầu tiên của container đầu tiên là tốt nhất => gán nó là best, tính điểm và gán điểm nó là bestScore
            Timetable best = sampleContainer[0].Timetables[0];
            int bestScore = EvaluationFitness(ref best, subjects, teacherAssignedLessons);

            Console.WriteLine("Lớp: {0} Section: {1} Start best score: {2}", timeTable.ClassInfo.Name, timeTable.Section, bestScore);

            for (int i = 0; i < best.Err.Count; i++)
            {
                Console.WriteLine("Start - Lớp: {0}. Địa chỉ: {1}. Lỗi: {2}", best.Err[i].ClassName, best.Err[i].Address, best.Err[i].Reason);
            }

            // Nếu TKB đầu tiên có score = 0 => TKB tốt nhất => return
            if (bestScore == 0)
            {
                timeTable = best;

                GetFinalTeacherAssignedLessons(timeTable, ref teacherAssignedLessons);

                var file = timeTable.ClassInfo.Name + "_" + timeTable.Section + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";

                ExportExcel(best.Lessons, file);

                return;
            }

            // Tạo danh sách TKB cha
            TimetableContainer selectedPopContainer = new TimetableContainer();
            // Enumerate generations
            for (int i = 0; i < n_pop; i++)
            {
                // Lấy TKB tốt nhất trong từng container bỏ vào ds TKB cha
                Timetable selectionList = Selection(sampleContainer[i].Timetables, subjects, teacherAssignedLessons);

                selectedPopContainer.Timetables.Add(selectionList);
            }

            for (int n_i = 0; n_i < n_iter; n_i++)
            {
                List<Timetable> children = new List<Timetable>();

                for (int i = 0; i < n_pop; i = i + 2)
                {
                    Timetable p1 = selectedPopContainer.Timetables[i];
                    Timetable p2 = selectedPopContainer.Timetables[i + 1];

                    // Crossover and mutation
                    List<Timetable> c = Crossover(p1, p2, r_cross); // Phối giống

                    for (int j = 0; j < c.Count; j++)
                    {
                        Timetable child = c[j];

                        Mutation(ref child, r_mut); // Đột biến

                        children.Add(child);
                    }
                }

                // Get result with best score
                Timetable temp = Selection(children, subjects, teacherAssignedLessons);

                // Chọn TKB có điểm thấp nhất trong danh sách TKB cha, thay thế = TKB con mới tìm được có điểm tốt hơn => cải thiện giống
                var tempMin = selectedPopContainer.Timetables[0].Score;
                int index = 0;
                for (int t = 1; t < selectedPopContainer.Timetables.Count; t++)
                {
                    if (selectedPopContainer.Timetables[t].Score < tempMin)
                    {
                        tempMin = selectedPopContainer.Timetables[t].Score;
                        index = t;
                    }
                }

                if (tempMin < temp.Score)
                {
                    selectedPopContainer.Timetables[index] = temp;
                }

                if (temp.Score > bestScore) Console.WriteLine("Best score: {0}", bestScore); // Log tracking

                // Nếu TKB bị lỗi -1 => Swap manual (chỉ với lỗi trùng tiết)
                if (temp.Score == -1)
                {
                    ManualSwap(ref temp, subjects, teacherAssignedLessons);
                }

                // Nếu TKB mới có điểm cao hơn TKB đầu vào => best = TKB mới
                if (temp.Score > bestScore)
                {
                    best = temp;
                    bestScore = temp.Score;
                };

                // Error tracking
                for (int i = 0; i < temp.Err.Count; i++)
                {
                    Console.WriteLine("Temp - Lớp: {0}. Địa chỉ: {1}. Lỗi: {2}", best.Err[i].ClassName, best.Err[i].Address, best.Err[i].Reason);
                }

                //if (bestScore < 0) n_i--; // Nếu điểm chưa = 0 => lùi biến chạy để chạy tới khi nào tìm được 0 thì thôi, đã comment lại
                if (bestScore == 0) break;
            }

            // Print result to screen
            Console.WriteLine("Lớp: {0}, Section: {1}, Best score: {2}", timeTable.ClassInfo.Name, timeTable.Section, bestScore);

            for (int i = 0; i < best.Err.Count; i++)
            {
                Console.WriteLine("Lớp: {0}. Địa chỉ: {1}. Lỗi: {2}", best.Err[i].ClassName, best.Err[i].Address, best.Err[i].Reason);
            }
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy"));
            Console.WriteLine();

            timeTable = best; // Gán best = TKB mới có số điểm tốt nhất

            // Lấy danh sách các tiết được xếp cho giáo viên của TKB tốt nhất
            GetFinalTeacherAssignedLessons(timeTable, ref teacherAssignedLessons);

            // Export excel để xem data
            var fileName = timeTable.ClassInfo.Name + "_" + timeTable.Section + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";

            ExportExcel(best.Lessons, fileName);
        }

        private static void ExportExcel(Lessons[,] temp, string fileName)
        {
            XLWorkbook workbook = new XLWorkbook();
            DataTable dt = new DataTable() { TableName = "New Worksheet" };
            DataSet ds = new DataSet();

            //input data
            var columns = new[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };

            var rows = new object[][]
             {
                 new object[] { temp[0, 0]?.Subject?.Name + " - " + temp[0, 0]?.Teacher?.Name,
                                temp[1, 0]?.Subject?.Name + " - " + temp[1, 0]?.Teacher?.Name,
                                temp[2, 0]?.Subject?.Name + " - " + temp[2, 0]?.Teacher?.Name,
                                temp[3, 0]?.Subject?.Name + " - " + temp[3, 0]?.Teacher?.Name,
                                temp[4, 0]?.Subject?.Name + " - " + temp[4, 0]?.Teacher?.Name,
                                temp[5, 0]?.Subject?.Name + " - " + temp[5, 0]?.Teacher?.Name},
                 new object[] { temp[0, 1]?.Subject?.Name + " - " + temp[0, 1]?.Teacher?.Name,
                                temp[1, 1]?.Subject?.Name + " - " + temp[1, 1]?.Teacher?.Name,
                                temp[2, 1]?.Subject?.Name + " - " + temp[2, 1]?.Teacher?.Name,
                                temp[3, 1]?.Subject?.Name + " - " + temp[3, 1]?.Teacher?.Name,
                                temp[4, 1]?.Subject?.Name + " - " + temp[4, 1]?.Teacher?.Name,
                                temp[5, 1]?.Subject?.Name + " - " + temp[5, 1]?.Teacher?.Name},
                 new object[] { temp[0, 2]?.Subject?.Name + " - " + temp[0, 2]?.Teacher?.Name,
                                temp[1, 2]?.Subject?.Name + " - " + temp[1, 2]?.Teacher?.Name,
                                temp[2, 2]?.Subject?.Name + " - " + temp[2, 2]?.Teacher?.Name,
                                temp[3, 2]?.Subject?.Name + " - " + temp[3, 2]?.Teacher?.Name,
                                temp[4, 2]?.Subject?.Name + " - " + temp[4, 2]?.Teacher?.Name,
                                temp[5, 2]?.Subject?.Name + " - " + temp[5, 2]?.Teacher?.Name},
                 new object[] { temp[0, 3]?.Subject?.Name + " - " + temp[0, 3]?.Teacher?.Name,
                                temp[1, 3]?.Subject?.Name + " - " + temp[1, 3]?.Teacher?.Name,
                                temp[2, 3]?.Subject?.Name + " - " + temp[2, 3]?.Teacher?.Name,
                                temp[3, 3]?.Subject?.Name + " - " + temp[3, 3]?.Teacher?.Name,
                                temp[4, 3]?.Subject?.Name + " - " + temp[4, 3]?.Teacher?.Name,
                                temp[5, 3]?.Subject?.Name + " - " + temp[5, 3]?.Teacher?.Name},
                 new object[] { temp[0, 4]?.Subject?.Name + " - " + temp[0, 4]?.Teacher?.Name,
                                temp[1, 4]?.Subject?.Name + " - " + temp[1, 4]?.Teacher?.Name,
                                temp[2, 4]?.Subject?.Name + " - " + temp[2, 4]?.Teacher?.Name,
                                temp[3, 4]?.Subject?.Name + " - " + temp[3, 4]?.Teacher?.Name,
                                temp[4, 4]?.Subject?.Name + " - " + temp[4, 4]?.Teacher?.Name,
                                temp[5, 4]?.Subject?.Name + " - " + temp[5, 4]?.Teacher?.Name},
             };

            //Add columns
            dt.Columns.AddRange(columns.Select(c => new DataColumn(c)).ToArray());

            //Add rows
            foreach (var row in rows)
            {
                dt.Rows.Add(row);
            }

            //Convert datatable to dataset and add it to the workbook as worksheet
            ds.Tables.Add(dt);
            workbook.Worksheets.Add(ds);

            //save
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savePath = Path.Combine(desktopPath, fileName);
            workbook.SaveAs(savePath, false);
        }

        private static void GetFinalTeacherAssignedLessons(Timetable sample, ref List<TeacherAssignedLessonsInfo> teacherAssignedLessons)
        {
            List<TeacherAssignedLessonsInfo> result = new List<TeacherAssignedLessonsInfo>();

            List<MaximumLessons> trackingML = new List<MaximumLessons>();
            Lessons[,] lessons = sample.Lessons;

            // Loop through row (first dimenson) => Thứ
            for (int row = 0; row < lessons.GetLength(0); row++)
            {

                for (int column = 0; column < lessons.GetLength(1); column++)
                {
                    if (lessons[row, column] == null) continue;

                    string address = row.ToString() + "_" + column.ToString();

                    // RULE 1. Not duplicate lessons same teacher
                    var td = result.Find(x => x.TeacherId == lessons[row, column].Teacher.Id);

                    if (td == null)
                    {
                        TeacherAssignedLessonsInfo tmpTd = new TeacherAssignedLessonsInfo();
                        tmpTd.TeacherId = lessons[row, column].Teacher.Id;
                        tmpTd.AssignedLessonInfos = new List<AssignedLessonInfo>();
                        tmpTd.AssignedLessonInfos.Add(new AssignedLessonInfo(address, sample.ClassInfo.Id, sample.ClassInfo.Name, sample.Section));

                        result.Add(tmpTd);
                    }
                    else
                    {
                        var info = td.AssignedLessonInfos.Find(x => x.LessonAddress == address && x.ClassId != sample.ClassInfo.Id);

                        if (info == null)
                        {
                            td.AssignedLessonInfos.Add(new AssignedLessonInfo(address, sample.ClassInfo.Id, sample.ClassInfo.Name, sample.Section));
                        }
                    }
                }
            }

            teacherAssignedLessons = result;
        }

        // Chỉ work khi TKB có score = -1 và lỗi là trùng tiết
        // Chạy tuần tự từ đầu đến cuối TKB, Swap tiết lỗi với tiết mới => đánh điểm lại, nếu < 0 => trả lại tiết lỗi về vị trí cũ => chạy tiếp
        private static void ManualSwap(ref Timetable timetable, List<SubjectInfo> subjects, List<TeacherAssignedLessonsInfo> teacherAssignedLessonsInfos)
        {
            // Find error address
            var e = timetable.Err.Find(x => x.ErrorType == 1);

            if (e != null)
            {
                var err_row = Convert.ToInt32(e.Address.Split('_')[0]);
                var err_col = Convert.ToInt32(e.Address.Split('_')[1]);

                for (int row = 0; row < timetable.Lessons.GetLength(0); row++)
                {
                    if (timetable.Score == 0) break;

                    for (int col = 0; col < timetable.Lessons.GetLength(1); col++)
                    {
                        if (row == err_row && col == err_col) continue;

                        var t = timetable.Lessons[row, col];

                        timetable.Lessons[row, col] = timetable.Lessons[err_row, err_col];
                        timetable.Lessons[err_row, err_col] = t;

                        // Evalutation Fitness again
                        int newScore = EvaluationFitness(ref timetable, subjects, teacherAssignedLessonsInfos);

                        if (newScore == 0)
                        {
                            timetable.Score = newScore;
                            timetable.Err = new List<TrackingError>();
                            break;
                        }
                        else
                        {
                            timetable.Lessons[err_row, err_col] = timetable.Lessons[row, col];
                            timetable.Lessons[row, col] = t;
                        }
                    }
                }
            }
        }
    }
}