using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoGA
{
    public static class Functions
    {
        public static List<SampleModelList> GetInput(int n_pop)
        {
            List<SampleModelList> result = new List<SampleModelList>();

            // Get samples
            SampleModelList list1 = new SampleModelList();

            result.AddRange(new List<SampleModelList> { list1 });

            for (int i = 0; i < n_pop; i++)
            {
                SampleModelList newList = new SampleModelList();
                newList.Models = SampleModelList.Shuffle(new Random(), result[i].Models);
                result.AddRange(new List<SampleModelList> { newList });
            }

            return result;
        }

        public static int EvaluationFitness(SampleModelList sample)
        {
            int score = 0;
            List<MaximumLessonInfo> maxLessons = new List<MaximumLessonInfo>();

            // Loop through row (first dimenson) => Thứ
            for (int row = 0; row < sample.Models.GetLength(0); row++)
            {
                string currentLesson = "";
                int currentLessonCount = 0;

                // Apply rule: maximum adjacent lessons
                for (int column = 0; column < sample.Models.GetLength(1); column++)
                {
                    if (currentLesson == sample.Models[row, column].SubjectName) currentLessonCount++;
                    else
                    {
                        currentLesson = sample.Models[row, column].SubjectName;
                        currentLessonCount = 1;
                    }

                    if (currentLessonCount >= Rule.MaximumContinous) score--;

                    // Count number of lesson
                    var lessonIndex = maxLessons.FindIndex(x => x.SubjectId == sample.Models[row, column].Id);

                    if (lessonIndex < 0)
                        maxLessons.Add(new MaximumLessonInfo(sample.Models[row, column].Id, 1));
                    else
                        maxLessons[lessonIndex].MaximumLesson++;
                }
            }

            // Apply rule: maximum lessons in timetable
            for (int i = 0; i < Rule.MaximumLessons.Count; i++)
            {
                var tmp = maxLessons.Find(x => x.SubjectId == Rule.MaximumLessons[i].SubjectId);

                if (tmp == null) score--;
                else if (tmp.MaximumLesson != Rule.MaximumLessons[i].MaximumLesson) score--;
            }

            return score;
        }

        public static SampleModelList Selection(List<SampleModelList> inputSamples)
        {
            foreach (var item in inputSamples)
            {
                item.Score = EvaluationFitness(item);
            }

            var qry = from p in inputSamples
                      orderby p.Score
                      select p;

            var temp = qry.ToArray();

            return temp[0];
        }

        public static List<SampleModelList> Crossover(SampleModelList p1, SampleModelList p2, double crossRate)
        {
            SampleModelList c1 = new SampleModelList();
            SampleModelList c2 = new SampleModelList();

            // Check for recombination
            Random rnd = new Random();
            if (rnd.NextDouble() < crossRate)
            {
                int pt = rnd.Next(1, p1.Models.GetLength(0) - 2);

                for (int i = 0; i < p1.Models.GetLength(0); i++)
                {
                    for (int j = 0; j < p1.Models.GetLength(1); j++)
                    {
                        if (i < pt)
                        {
                            c1.Models[i, j] = p1.Models[i, j];
                            c2.Models[i, j] = p2.Models[i, j];
                        }
                        else
                        {
                            c1.Models[i, j] = p2.Models[i, j];
                            c2.Models[i, j] = p1.Models[i, j];
                        }
                    }
                }
            }

            List<SampleModelList> result = new List<SampleModelList>();

            result.AddRange(new List<SampleModelList> { c1, c2 });

            return result;
        }

        public static void Mutation(ref SampleModelList sample, double mutationRate)
        {
            for (int i = 1; i < sample.Models.Length; i++)
            {
                Random rnd = new Random();
                if (rnd.NextDouble() < mutationRate)
                {
                    for (int row = 0; row < sample.Models.GetLength(0); row++)
                    {
                        for (int col = 1; col < sample.Models.GetLength(1); col++)
                        {
                            var temp = sample.Models[row, col];
                            sample.Models[row, col] = sample.Models[row, col - 1];
                            sample.Models[row, col - 1] = temp;
                        }
                    }
                }
            }
        }

        public static void GeneticAlgorithm(int n_iter, int n_pop, double r_cross, double r_mut)
        {
            // Get input sample list
            int n_pop_loop = n_pop / 2;
            if (n_pop_loop < 2) n_pop_loop = 2;

            List<SampleModelContainer> sampleContainer = new List<SampleModelContainer>();

            for (int i = 0; i < n_pop; i++)
            {
                List<SampleModelList> input = GetInput(n_pop_loop);

                sampleContainer.Add(new SampleModelContainer(input));
            }

            // Tracking best solution
            SampleModelList best = sampleContainer[0].ListSample[0];
            int bestScore = EvaluationFitness(sampleContainer[0].ListSample[0]);

            SampleModelContainer selectedPopContainer = new SampleModelContainer();
            // Enumerate generations
            for (int i = 0; i < n_pop; i++)
            {
                SampleModelList selectionList = Selection(sampleContainer[i].ListSample);

                selectedPopContainer.ListSample.Add(selectionList);
            }

            for (int n_i = 0; n_i < n_iter; n_i++)
            {
                List<SampleModelList> children = new List<SampleModelList>();

                for (int i = 0; i < n_pop; i = i + 2)
                {
                    SampleModelList p1 = selectedPopContainer.ListSample[i];
                    SampleModelList p2 = selectedPopContainer.ListSample[i + 1];

                    // Crossover and mutation
                    List<SampleModelList> c = Crossover(p1, p2, r_cross);

                    for (int j = 0; j < c.Count; j++)
                    {
                        SampleModelList child = c[j];

                        Mutation(ref child, r_mut);

                        children.Add(child);
                    }
                }

                // Get result with best score
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Score = EvaluationFitness(children[i]);
                }

                SampleModelList temp = Selection(children);

                if (temp.Score >= bestScore) best = temp;
            }

            Console.WriteLine("Best score: {0}", bestScore);

            // Print result to screen
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Thứ 2 || Thứ 3 || Thứ 4 || Thứ 5 || Thứ 6 || Thứ 7");

            for (int i = 0; i < best.Models.GetLength(1); i++)
            {
                SampleModel[,] temp = best.Models;

                Console.WriteLine("{0} || {1} || {2} || {3} || {4} || {5}", temp[0, i].SubjectName, temp[1, i].SubjectName, temp[2, i].SubjectName, temp[3, i].SubjectName, temp[4, i].SubjectName, temp[5, i].SubjectName);
            }

            //
        }
    }
}