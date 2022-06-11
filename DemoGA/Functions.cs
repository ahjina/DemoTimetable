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

        public static List<Timetable> GetInput(int n_pop, ClassInfo classInfo, List<TeacherInfo> teachers, List<TeachingDistribution> teachingDistributions)
        {
            List<Timetable> result = new List<Timetable>();

            // Get samples
            Lessons[,] lessons = InitData.GetInputLessons(classInfo.Subjects, teachers, teachingDistributions);

            for (int i = 0; i < n_pop; i++)
            {
                Timetable t = new Timetable(classInfo);
                t.Lessons = InitData.Shuffle(new Random(), lessons);

                AssignFixedLessons(classInfo.Subjects, ref t);

                result.Add(t);
            }

            return result;
        }

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

        public static int EvaluationFitness(Timetable sample, ref List<TeacherAssignedLessonsInfo> teacherAssignedLessons, ref List<TrackingError> err)
        {
            int score = 0;

            List<MaximumLessons> trackingML = new List<MaximumLessons>();
            Lessons[,] lessons = sample.Lessons;

            // Loop through row (first dimenson) => Thứ
            for (int row = 0; row < lessons.GetLength(0); row++)
            {
                int currentLessonId = 0;
                int currentLessonCount = 0;

                for (int column = 0; column < lessons.GetLength(1); column++)
                {
                    if (lessons[row, column] == null) continue;

                    string address = row.ToString() + "_" + column.ToString();

                    // RULE 1. Not duplicate lessons same teacher
                    var td = teacherAssignedLessons.Find(x => x.TeacherId == lessons[row, column].Teacher.Id);

                    if (td == null)
                    {
                        TeacherAssignedLessonsInfo tmpTd = new TeacherAssignedLessonsInfo();
                        tmpTd.TeacherId = lessons[row, column].Teacher.Id;
                        tmpTd.AssignedLessons.Add(address);

                        teacherAssignedLessons.Add(tmpTd);
                    }
                    else if (td.AssignedLessons.Contains(address) && lessons[row, column].IsLock == 0)
                    {
                        score--;
                        var e = new TrackingError();
                        e.ClassName = sample.ClassInfo.Name;
                        e.Address = address;
                        e.Reason = String.Format("Trùng tiết của gv {0} môn {1}", lessons[row, column].Teacher.Name, lessons[row, column].Subject.Name);

                        err.Add(e);
                    }
                    else if (!td.AssignedLessons.Contains(address)) td.AssignedLessons.Add(address);

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
                        e.Reason = String.Format("Vượt quá số tiết liên tiếp môn {0}. Tồng số tiết {1}", lessons[row, column].Subject.Name, currentLessonCount);

                        err.Add(e);
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
            for (int i = 0; i < sample.ClassInfo.Subjects.Count; i++)
            {
                var tmp = trackingML.Find(x => x.SubjectId == sample.ClassInfo.Subjects[i].Id);

                if (tmp == null)
                {
                    score--;
                    var e = new TrackingError();
                    e.ClassName = sample.ClassInfo.Name;
                    e.Reason = String.Format("Thiếu môn {0}", sample.ClassInfo.Subjects[i].Id);

                    err.Add(e);
                }
                else if (tmp.CurentLessons != sample.ClassInfo.Subjects[i].LessonsPerWeek)
                {
                    score--;
                    var e = new TrackingError();
                    e.ClassName = sample.ClassInfo.Name;
                    e.Reason = String.Format("Không đủ số tiết 1 tuần môn {0}", sample.ClassInfo.Subjects[i].Id);

                    err.Add(e);
                }
            }

            return score;
        }

        public static Timetable Selection(List<Timetable> inputSamples, ref List<TeacherAssignedLessonsInfo> teacherAssignedLessonsInfos, ref List<TrackingError> err)
        {
            foreach (var item in inputSamples)
            {
                item.Score = EvaluationFitness(item, ref teacherAssignedLessonsInfos, ref err);
            }

            var qry = from p in inputSamples
                      orderby p.Score
                      select p;

            var temp = qry.ToArray();

            return temp[0];
        }

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
                        if (p1.Lessons[i, j].IsLock == 1)
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

        public static void Mutation(ref Timetable sample, double mutationRate)
        {
            for (int i = 1; i < sample.Lessons.Length; i++)
            {
                Random rnd = new Random();
                double rndDouble = rnd.NextDouble();

                if (rndDouble < mutationRate)
                {
                    for (int row = 0; row < sample.Lessons.GetLength(0); row++)
                    {
                        for (int col = 1; col < sample.Lessons.GetLength(1); col++)
                        {
                            var temp = sample.Lessons[row, col];
                            var prev = sample.Lessons[row, col - 1];

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
            int n_pop_loop = n_pop / 2;
            if (n_pop_loop < 2) n_pop_loop = 2;

            int classId = timeTable.ClassInfo.Id;
            List<TeacherInfo> teachers = InitData.GetListTeacher();
            List<TeachingDistribution> teachingDistributions = InitData.GetTeachingDistributions();
            List<TeachingDistribution> td = teachingDistributions.Where(x => x.ClassessId.Any(y => y == classId)).ToList();

            List<TimetableContainer> sampleContainer = new List<TimetableContainer>();

            for (int i = 0; i < n_pop; i++)
            {
                List<Timetable> input = GetInput(n_pop, timeTable.ClassInfo, teachers, td);

                sampleContainer.Add(new TimetableContainer(input));
            }

            // Tracking best solution
            List<TrackingError> err = new List<TrackingError>();

            Timetable best = sampleContainer[0].Timetables[0];
            int bestScore = EvaluationFitness(sampleContainer[0].Timetables[0], ref teacherAssignedLessons, ref err);
            Console.WriteLine("Lớp: {0}, Best score: {1}", timeTable.ClassInfo.Name, bestScore);

            for (int i = 0; i < err.Count; i++)
            {
                Console.WriteLine("Lớp: {0}. Địa chỉ: {1}. Lỗi: {2}", err[i].ClassName, err[i].Address, err[i].Reason);
            }

            TimetableContainer selectedPopContainer = new TimetableContainer();
            // Enumerate generations
            for (int i = 0; i < n_pop; i++)
            {
                err = new List<TrackingError>();
                Timetable selectionList = Selection(sampleContainer[i].Timetables, ref teacherAssignedLessons, ref err);

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
                    List<Timetable> c = Crossover(p1, p2, r_cross);

                    for (int j = 0; j < c.Count; j++)
                    {
                        Timetable child = c[j];

                        Mutation(ref child, r_mut);

                        children.Add(child);
                    }
                }

                // Get result with best score
                err = new List<TrackingError>();
                Timetable temp = Selection(children, ref teacherAssignedLessons, ref err);

                if (temp.Score > bestScore)
                {
                    best = temp;
                    bestScore = temp.Score;

                    Console.WriteLine("Best score: {0}", bestScore);

                    for (int i = 0; i < err.Count; i++)
                    {
                        Console.WriteLine("Lớp: {0}. Địa chỉ: {1}. Lỗi: {2}", err[i].ClassName, err[i].Address, err[i].Reason);
                    }
                };
            }

            // Print result to screen
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Lớp {0}, Best score: {1}", timeTable.ClassInfo.Name, bestScore);
            Console.WriteLine();

            timeTable = best;

            GetFinalTeacherAssignedLessons(timeTable, ref teacherAssignedLessons);

            var fileName = timeTable.ClassInfo.Name + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";

            ExportExcel(best.Lessons, fileName);
        }

        public static void ExportExcel(Lessons[,] temp, string fileName)
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

        public static void GetFinalTeacherAssignedLessons(Timetable sample, ref List<TeacherAssignedLessonsInfo> teacherAssignedLessons)
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
                        tmpTd.AssignedLessons.Add(address);

                        result.Add(tmpTd);
                    }
                    else if (!td.AssignedLessons.Contains(address)) td.AssignedLessons.Add(address);
                }
            }

            teacherAssignedLessons = result;
        }
    }
}