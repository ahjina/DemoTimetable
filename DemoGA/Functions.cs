using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                result.Add(t);
            }

            return result;
        }

        public static int EvaluationFitness(Timetable sample, ref List<TeacherAssignedLessonsInfo> teacherAssignedLessons)
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
                    else if (td.AssignedLessons.Contains(address)) score--;
                    else td.AssignedLessons.Add(address);

                    // RULE 2. Check maximum continous lessons
                    if (currentLessonId == lessons[row, column].Subject.Id) currentLessonCount++;
                    else
                    {
                        currentLessonId = lessons[row, column].Subject.Id;
                        currentLessonCount = 1;
                    }

                    if (currentLessonCount >= lessons[row, column].Subject.MaximumContinousLessons) score--;

                    // Count number of lesson

                    int index = trackingML.FindIndex(x => x.SubjectId == currentLessonId);

                    if (index < 0)
                        trackingML.Add(new MaximumLessons(lessons[row, column].Id, 1));
                    else
                        trackingML[index].CurentLessons++;
                }
            }

            // RULE 3. Lessons per week
            for (int i = 0; i < sample.ClassInfo.Subjects.Count; i++)
            {
                var tmp = trackingML.Find(x => x.SubjectId == sample.ClassInfo.Subjects[i].Id);

                if (tmp == null) score--;
                else if (tmp.CurentLessons != sample.ClassInfo.Subjects[i].LessonsPerWeek) score--;
            }

            return score;
        }

        public static Timetable Selection(List<Timetable> inputSamples, ref List<TeacherAssignedLessonsInfo> teacherAssignedLessonsInfos)
        {
            foreach (var item in inputSamples)
            {
                item.Score = EvaluationFitness(item, ref teacherAssignedLessonsInfos);
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
                int pt = rnd.Next(1, p1.Lessons.GetLength(0) - 2);

                for (int i = 0; i < p1.Lessons.GetLength(0); i++)
                {
                    for (int j = 0; j < p1.Lessons.GetLength(1); j++)
                    {
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
                            sample.Lessons[row, col] = sample.Lessons[row, col - 1];
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
            Timetable best = sampleContainer[0].Timetables[0];
            int bestScore = EvaluationFitness(sampleContainer[0].Timetables[0], ref teacherAssignedLessons);

            TimetableContainer selectedPopContainer = new TimetableContainer();
            // Enumerate generations
            for (int i = 0; i < n_pop; i++)
            {
                Timetable selectionList = Selection(sampleContainer[i].Timetables, ref teacherAssignedLessons);

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
                Timetable temp = Selection(children, ref teacherAssignedLessons);

                if (temp.Score >= bestScore) best = temp;
            }

            Console.WriteLine("Best score: {0}", bestScore);

            // Print result to screen
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Lớp {0}", timeTable.ClassInfo.Name);
            Console.WriteLine("Thứ 2        || Thứ 3        || Thứ 4        || Thứ 5        || Thứ 6        || Thứ 7");

            for (int i = 0; i < best.Lessons.GetLength(1); i++)
            {
                Lessons[,] temp = best.Lessons;

                Console.WriteLine("{0}      || {1}      || {2}      || {3}      || {4}      || {5}", 
                    temp[0, i].Subject.Name + " - " + temp[0, i].Teacher.Name, 
                    temp[1, i].Subject.Name + " - " + temp[1, i].Teacher.Name, 
                    temp[2, i].Subject.Name + " - " + temp[2, i].Teacher.Name, 
                    temp[3, i].Subject.Name + " - " + temp[3, i].Teacher.Name, 
                    temp[4, i].Subject.Name + " - " + temp[4, i].Teacher.Name, 
                    temp[5, i].Subject.Name + " - " + temp[5 , i].Teacher.Name);
            }

            timeTable = best;
        }
    }
}