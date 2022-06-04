using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoGA
{
    /*
     ========== MODELS ==========
    grade {
        classes [
            {
                id
                name
                main-session: string (morning / afternoon)
                off-lessons: string[] (list "row-col")
                subject-info {
                    id
                    teacher-id
                }
            }
        ]
        subjects [
            id
            name
            teacher-info {
                id
                name
                ... (more config - later)
            }
            lessons-per-week
            maximum-continous-lessons
            fixed-lesson: string[] (list "row-col")
            minimum-lesson-per-session [
                section: string (morning / afternoon)
                num-of-lessons
            ]
            subject-department: string (science / social)
        ]
    }

     */
    public class SampleModel
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }

        public SampleModel(int id, string subjectName)
        {
            this.Id = id;
            this.SubjectName = subjectName;
        }
    }

    public class SampleModelList
    {
        public SampleModel[,] Models { get; set; }
        public int Score { get; set; }

        public SampleModelList() => InitList();

        private void InitList()
        {
            SampleModel[,] models = new SampleModel[6, 5];
            models[0, 0] = new SampleModel(1, "Chào cờ");
            models[0, 1] = new SampleModel(2, "SHL");
            models[0, 2] = new SampleModel(3, "Hóa");
            models[0, 3] = new SampleModel(4, "Sử");
            models[0, 4] = new SampleModel(5, "Lý");
            models[1, 0] = new SampleModel(6, "Văn");
            models[1, 1] = new SampleModel(6, "Văn");
            models[1, 2] = new SampleModel(5, "Lý");
            models[1, 3] = new SampleModel(7, "Anh");
            models[1, 4] = new SampleModel(8, "Toán");
            models[2, 0] = new SampleModel(8, "Toán");
            models[2, 1] = new SampleModel(8, "Toán");
            models[2, 2] = new SampleModel(5, "Lý");
            models[2, 3] = new SampleModel(6, "Văn");
            models[2, 4] = new SampleModel(5, "Lý");
            models[3, 0] = new SampleModel(7, "Anh");
            models[3, 1] = new SampleModel(7, "Anh");
            models[3, 2] = new SampleModel(6, "Văn");
            models[3, 3] = new SampleModel(6, "Văn");
            models[3, 4] = new SampleModel(8, "Toán");
            models[4, 0] = new SampleModel(8, "Toán"); 
            models[4, 1] = new SampleModel(8, "Toán");
            models[4, 2] = new SampleModel(3, "Hóa"); 
            models[4, 3] = new SampleModel(3, "Hóa");
            models[4, 4] = new SampleModel(10, "Sinh");
            models[5, 0] = new SampleModel(10, "Sinh");
            models[5, 1] = new SampleModel(4, "Sử");
            models[5, 2] = new SampleModel(3, "Hóa");
            models[5, 3] = new SampleModel(9, "Địa");
            models[5, 4] = new SampleModel(9, "Địa");

            this.Models = models;
        }

        public static T[,] Shuffle<T>(Random random, T[,] array)
        {
            int lengthRow = array.GetLength(1);

            for (int i = array.Length - 1; i > 0; i--)
            {
                int i0 = i / lengthRow;
                int i1 = i % lengthRow;

                int j = random.Next(i + 1);
                int j0 = j / lengthRow;
                int j1 = j % lengthRow;

                T temp = array[i0, i1];
                array[i0, i1] = array[j0, j1];
                array[j0, j1] = temp;
            }

            return array;
        }
    }

    public class SampleModelContainer
    {
        public List<SampleModelList> ListSample { get; set; }

        public SampleModelContainer()
        {
            this.ListSample = new List<SampleModelList>();
        }

        public SampleModelContainer(List<SampleModelList> list)
        {
            this.ListSample = list;
        }
    }

    public class MaximumLessonInfo
    {
        public int SubjectId { get; set; }
        public int MaximumLesson { get; set; }

        public MaximumLessonInfo() { }

        public MaximumLessonInfo(int subjectId, int maximumLesson)
        {
            this.SubjectId = subjectId;
            this.MaximumLesson = maximumLesson;
        }

        public List<MaximumLessonInfo> GetMaximumLessonInfo()
        {
            return new List<MaximumLessonInfo>
            {
                new MaximumLessonInfo(1, 1),
                new MaximumLessonInfo(2, 1),
                new MaximumLessonInfo(3, 4),
                new MaximumLessonInfo(4, 2),
                new MaximumLessonInfo(5, 4),
                new MaximumLessonInfo(6, 5),
                new MaximumLessonInfo(7, 3),
                new MaximumLessonInfo(8, 6),
                new MaximumLessonInfo(9, 2),
                new MaximumLessonInfo(10, 2),
            };
        }
    }

    public static class Rule
    {
        public static int MaximumContinous = 3;
        public static List<MaximumLessonInfo> MaximumLessons = new MaximumLessonInfo().GetMaximumLessonInfo();
    }
}
