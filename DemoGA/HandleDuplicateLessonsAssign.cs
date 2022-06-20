using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoGA
{
    public static class HandleDuplicateLessonsAssign
    {
        private static void MiniSwap(ref Lessons a, ref Lessons b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        private static List<TrackingAssignedLessons> GetTrackingAssignedLessons(Timetable timetable, ref List<TrackingAssignedLessons> trackingAssignedSingleLessons)
        {
            List<TrackingAssignedLessons> result = new List<TrackingAssignedLessons>();

            for (int row = 0; row < timetable.Lessons.GetLength(0); row++)
            {
                for (int column = 0; column < timetable.Lessons.GetLength(1); column++)
                {
                    var lesson = timetable.Lessons[row, column];

                    if (lesson != null && lesson.Subject.HasDuplicateLessons)
                    {
                        // Find tracking info
                        var index = result.FindIndex(x => x.SubjectId == lesson.Subject.Id);

                        if (index < 0)
                        {
                            TrackingAssignedLessons tmp = new TrackingAssignedLessons();
                            tmp.SubjectId = lesson.Subject.Id;
                            tmp.SubjectName = lesson.Subject.Name;
                            tmp.TotalLessons = lesson.Subject.LessonsPerWeek;

                            // Check if address is exists
                            var ti = tmp.LessonsAddress.Find(x => x.row == row && x.col == column);

                            if (ti == null)
                                tmp.LessonsAddress.Add(new LessonAddress(row, column));

                            result.Add(tmp);
                        }
                        else
                        {
                            // Check if address is exists
                            var ti = result[index].LessonsAddress.Find(x => x.row == row && x.col == column);

                            if (ti == null)
                                result[index].LessonsAddress.Add(new LessonAddress(row, column));
                        }
                    }
                    else if (lesson != null && lesson.IsLock == 0)
                    {
                        var index2 = trackingAssignedSingleLessons.FindIndex(x => x.SubjectId == lesson.Subject.Id);

                        if (index2 < 0)
                        {
                            TrackingAssignedLessons tmp = new TrackingAssignedLessons();
                            tmp.SubjectId = lesson.Subject.Id;
                            tmp.SubjectName = lesson.Subject.Name;

                            // Check if address is exists
                            var ti = tmp.LessonsAddress.Find(x => x.row == row && x.col == column);

                            if (ti == null)
                                tmp.LessonsAddress.Add(new LessonAddress(row, column));

                            trackingAssignedSingleLessons.Add(tmp);
                        }
                        else
                        {
                            // Check if address is exists
                            var ti = result[index2].LessonsAddress.Find(x => x.row == row && x.col == column);

                            if (ti == null)
                                trackingAssignedSingleLessons[index2].LessonsAddress.Add(new LessonAddress(row, column));
                        }

                    }
                }
            }

            return result;
        }

        // Suppose input subjects is available subjects (subject has no duplicate lessons)
        private static List<ReferenceLessons> GetListReferenceLessons(LessonAddress lessonForCheck, Lessons[,] lessons, List<int> availableSubjects)
        {
            List<ReferenceLessons> result = new List<ReferenceLessons>();

            var previousLessonAddress = new LessonAddress(lessonForCheck.row, lessonForCheck.col - 1);
            var nextLessonAddress = new LessonAddress(lessonForCheck.row, lessonForCheck.col + 1);

            if (lessonForCheck.col > 0)
            {
                var rf = GetReferenceLesson(lessonForCheck, previousLessonAddress, lessons, availableSubjects, lessons[lessonForCheck.row, lessonForCheck.col].Subject.Id);
                if (rf != null) result.Add(rf);
            }

            if (lessonForCheck.col < InitData.LESSONSPERSECTION - 1)
            {
                var rf = GetReferenceLesson(lessonForCheck, nextLessonAddress, lessons, availableSubjects, lessons[lessonForCheck.row, lessonForCheck.col].Subject.Id);
                if (rf != null) result.Add(rf);
            }

            return result;
        }

        private static ReferenceLessons? GetReferenceLesson(LessonAddress baseLesson, LessonAddress refLesson, Lessons[,] Lessons, List<int> availableSubjects, int subjectId)
        {
            var lessonForCheckInfo = Lessons[refLesson.row, refLesson.col];

            if (lessonForCheckInfo == null)
                return new ReferenceLessons(refLesson, baseLesson, subjectId);

            if (availableSubjects.Contains(lessonForCheckInfo.Subject.Id))
            {
                if (lessonForCheckInfo.IsLock == 0)
                    return new ReferenceLessons(refLesson, baseLesson, subjectId);
            }

            return null;
        }

        private static List<AssignedDuplicateLessonsInfo> GetAssignedDuplicateLessonsInfo(Timetable timetable, List<TrackingAssignedLessons> listTracking, List<SubjectInfo> subjects, ref List<ReferenceLessons> referenceLessons)
        {
            List<AssignedDuplicateLessonsInfo> result = new List<AssignedDuplicateLessonsInfo>();

            var lessons = timetable.Lessons;

            // Get list available subjects (subject has no duplicate lessons)c
            var listS = subjects.Where(x => x.HasDuplicateLessons == false).Select(x => x.Id).Distinct().ToList();

            foreach (var trackingItem in listTracking)
            {
                int maximumPair = trackingItem.TotalLessons / 2;
                int currentPair = 0;

                // Find assigned double lessons info               

                var listA = trackingItem.LessonsAddress.GroupBy(x => x.row,
                    (key, subList) => new
                    {
                        Key = key,
                        SubList = subList.OrderBy(x => x.row).ToList()
                    }).OrderBy(x => x.Key).ToList();

                for (int i = 0; i < listA.Count; i++)
                {
                    if (listA[i].SubList.Count == 1)
                    {
                        var currentLesson = lessons[listA[i].SubList[0].row, listA[i].SubList[0].col];

                        var index = result.FindIndex(x => x.SubjectId == currentLesson.Subject.Id);

                        if (index < 0)
                        {
                            AssignedDuplicateLessonsInfo tmp = new AssignedDuplicateLessonsInfo();
                            tmp.SubjectId = currentLesson.Subject.Id;
                            tmp.CurrentPair = currentPair;
                            tmp.MaximumPair = maximumPair;
                            tmp.SingleAddress.Add(listA[i].SubList[0]);

                            result.Add(tmp);
                        }
                        else
                        {
                            result[index].SingleAddress.Add(listA[i].SubList[0]);
                        }

                        var listTmp = GetListReferenceLessons(listA[i].SubList[0], lessons, listS);

                        if (listTmp != null && listTmp.Count > 0) referenceLessons.AddRange(listTmp);
                    }
                    else
                    {
                        for (int j = 0; j < listA[i].SubList.Count;)
                        {
                            var currentLesson = lessons[listA[i].SubList[j].row, listA[i].SubList[j].col];

                            if (j < listA[i].SubList.Count - 1)
                            {
                                var col1 = listA[i].SubList[j].col;
                                var col2 = listA[i].SubList[j + 1].col;

                                if (col1 == col2 + 1 || col2 == col1 + 1)
                                {
                                    currentPair++;
                                    j = j + 2;

                                    var index = result.FindIndex(x => x.SubjectId == currentLesson.Subject.Id);

                                    if (index < 0)
                                    {
                                        AssignedDuplicateLessonsInfo tmp = new AssignedDuplicateLessonsInfo();
                                        tmp.SubjectId = currentLesson.Subject.Id;
                                        tmp.CurrentPair = currentPair;
                                        tmp.MaximumPair = maximumPair;

                                        result.Add(tmp);
                                    }
                                    else
                                    {
                                        result[index].CurrentPair = currentPair;
                                    }

                                    continue;
                                }
                            }

                            var index2 = result.FindIndex(x => x.SubjectId == currentLesson.Subject.Id);

                            if (index2 < 0)
                            {
                                AssignedDuplicateLessonsInfo tmp = new AssignedDuplicateLessonsInfo();
                                tmp.SubjectId = currentLesson.Subject.Id;
                                tmp.CurrentPair = currentPair;
                                tmp.MaximumPair = maximumPair;
                                tmp.SingleAddress.Add(listA[i].SubList[j]);

                                result.Add(tmp);
                            }
                            else
                            {
                                result[index2].SingleAddress.Add(listA[i].SubList[j]);
                            }

                            var listTmp = GetListReferenceLessons(listA[i].SubList[j], lessons, listS);

                            if (listTmp != null && listTmp.Count > 0) referenceLessons.AddRange(listTmp);

                            j++;

                        }
                    }
                }
            }

            return result;
        }

        public static void AssignDuplicateLessons(Timetable timetable, List<AssignedDuplicateLessonsInfo> listADL, List<ReferenceLessons> listRL, List<LessonAddress> trackingASL)
        {
            var newTimetable = timetable;
            foreach (var adl in listADL)
            {
                int A = adl.MaximumPair - adl.CurrentPair;
                var B = adl.SingleAddress;

                if (A > 0)
                {
                    var C = listRL.Where(x => x.SubjectId == adl.SubjectId).ToList();

                    for (int i = A; i <= (B.Count - i) * 2; i++)
                    {
                        var E = Combinations(B, i).ToList();

                        var J = new List<LessonAddress>();
                        // Get list address for swap
                        for (int t = 0; t < i; t++)
                        {
                            J.Add(B[t]);
                        }

                        for (int j = 0; j < E.Count; j++)
                        {
                            var filteredC = C.Where(x => !J.Contains(x.RefAddress)).ToList();
                            var _fC = filteredC.Select(x => x.Address).ToList();
                            var t = _fC;

                            if (i > 1) t = _fC.Concat(trackingASL).DistinctBy(x => x.row + "|" + x.col).ToList();


                            if (t.Count < E[j].ToList().Count) continue;

                            var F = Combinations(t, E[j].ToList().Count).ToList();

                            for (int p = 0; p < F.Count; p++)
                            {
                                var tmp = F[p].ToList();
                                for (int q = 0; q < tmp.Count; q++)
                                {
                                    var E1 = E[j].ToList()[q];

                                    MiniSwap(ref newTimetable.Lessons[E1.row, E1.col], ref newTimetable.Lessons[tmp[q].row, tmp[q].col]);
                                }
                                // Evaluation

                                // Export excel 
                                var fileName = string.Format("timetable_{0}_{1}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmssfff"), i);

                                Functions.ExportExcel(timetable.Lessons, fileName);

                                newTimetable = timetable;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("== end ==");
        }

        public static void RunSample()
        {
            Timetable sample = new Timetable();
            sample.Lessons = GetSampleLessons();

            List<TrackingAssignedLessons> trackingASL = new List<TrackingAssignedLessons>();
            List<SubjectInfo> subjects = InitData.GetListSubject(InitData.MORNING_SECTION);
            List<ReferenceLessons> listRL = new List<ReferenceLessons>();
            List<TrackingAssignedLessons> trackingADL = GetTrackingAssignedLessons(sample, ref trackingASL);
            List<AssignedDuplicateLessonsInfo> listADL = GetAssignedDuplicateLessonsInfo(sample, trackingADL, subjects, ref listRL);

            var assignedSingleLessons = trackingASL.SelectMany(x => x.LessonsAddress).ToList();

            AssignDuplicateLessons(sample, listADL, listRL, assignedSingleLessons);
        }

        private static Lessons[,] GetSampleLessons()
        {
            List<SubjectInfo> subjects = InitData.GetListSubject(InitData.MORNING_SECTION);

            Lessons[,] result = new Lessons[6, 5]
            {
                { new Lessons(1, subjects[0], 1), new Lessons(2, subjects[1], 1), new Lessons(3, subjects[5]), null, new Lessons(5, subjects[5]) },
                { new Lessons(6, subjects[6]), new Lessons(7, subjects[8]), new Lessons(8, subjects[5]), new Lessons(9, subjects[3]), new Lessons(10, subjects[5]) },
                { new Lessons(11, subjects[11]), new Lessons(12, subjects[7]), new Lessons(13, subjects[6]), new Lessons(14, subjects[6]), new Lessons(15, subjects[7]) },
                { new Lessons(16, subjects[6]), new Lessons(17, subjects[6]), new Lessons(18, subjects[3]), new Lessons(19, subjects[3]), new Lessons(20, subjects[5]) },
                { new Lessons(21, subjects[4]), new Lessons(22, subjects[4]), new Lessons(23, subjects[2]), new Lessons(24, subjects[2]), new Lessons(25, subjects[9]) },
                { null, new Lessons(27, subjects[8]), new Lessons(28, subjects[4]), new Lessons(29, subjects[10]), null }
            };

            return result;
        }
    }
}
