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
                class-subject-info {
                    subject-info {
                        id
                        name
                        lessons-per-week
                        maximum-continous-lessons
                        fixed-lesson: string[] (list "row-col")
                        minimum-lesson-per-session [
                            {
                                section: string (morning / afternoon)
                                num-of-lessons
                            }
                        ]
                        subject-department: string (science / social)
                    }                    
                    teacher-info {
                        id
                        name
                        ... (more config - later)
                    }                    
                }
            }
        ]
    }

     */

    public class TeacherInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaximumLessons { get; set; }

        public TeacherInfo() { }

        public TeacherInfo(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

        public TeacherInfo(int Id, string Name, int? MaximumLessons)
        {
            this.Id = Id;
            this.Name = Name;
            this.MaximumLessons = MaximumLessons;
        }
    }

    public class LessonsPerSection
    {
        public string Section { get; set; }
        public int NumOfLessons { get; set; }

        public LessonsPerSection() { }

        public LessonsPerSection(string section, int numOfLessons)
        {
            Section = section;
            NumOfLessons = numOfLessons;
        }
    }

    public class SubjectInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int LessonsPerWeek { get; set; }
        public int MaximumContinousLessons { get; set; }
        public List<string>? FixedLessons { get; set; }
        public List<LessonsPerSection>? MinimumLessonsPerSection { get; set; }
        public string? SubjectDepartment { get; set; }

        public SubjectInfo()
        {
            FixedLessons = new List<string>();
            MinimumLessonsPerSection = new List<LessonsPerSection>();
        }

        public SubjectInfo(int id, string? name, int lessonsPerWeek, int maximumContinousLessons, List<string>? fixedLessons, List<LessonsPerSection>? minimumLessonsPerSection, string? subjectDepartment)
        {
            Id = id;
            Name = name;
            LessonsPerWeek = lessonsPerWeek;
            MaximumContinousLessons = maximumContinousLessons;
            FixedLessons = fixedLessons;
            MinimumLessonsPerSection = minimumLessonsPerSection;
            SubjectDepartment = subjectDepartment;
        }
    }

    public class ClassSubjectInfo
    {
        public SubjectInfo SubjectInfo { get; set; }
        public TeacherInfo TeacherInfo { get; set; }

        public ClassSubjectInfo()
        {
            SubjectInfo = new SubjectInfo();
            TeacherInfo = new TeacherInfo();
        }

        public ClassSubjectInfo(SubjectInfo subjectInfo, TeacherInfo teacherInfo)
        {
            SubjectInfo = subjectInfo;
            TeacherInfo = teacherInfo;
        }
    }

    public class ClassInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MainSection { get; set; }
        public List<string>? OffLessons { get; set; }
        public List<SubjectInfo> Subjects { get; set; }
        public TeacherInfo HeadTeacher { get; set; }

        public ClassInfo()
        {
            OffLessons = new List<string>();
            Subjects = new List<SubjectInfo>();
            HeadTeacher = new TeacherInfo();
        }
    }

    public class GradeInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ClassInfo> Classes { get; set; }

        public GradeInfo()
        {
            Classes = new List<ClassInfo>();
        }

        public GradeInfo(int id, string name, List<ClassInfo> classes)
        {
            Id = id;
            Name = name;
            Classes = classes;
        }
    }

    public class TeacherAssignedLessonsInfo
    {
        public int TeacherId { get; set; }
        public List<string> AssignedLessons { get; set; }

        public TeacherAssignedLessonsInfo()
        {
            AssignedLessons = new List<string>();
        }

        public TeacherAssignedLessonsInfo(int teacherId, List<string> assignedLessons)
        {
            TeacherId = teacherId;
            AssignedLessons = assignedLessons;
        }
    }

    public class TeachingDistribution
    {
        public int TeacherId { get; set; }
        public List<int> SubjectId { get; set; }
        public List<int> ClassessId { get; set; }

        public TeachingDistribution()
        {
            SubjectId = new List<int>();
            ClassessId = new List<int>();
        }

        public TeachingDistribution(int teacherId, List<int> subjectId, List<int> classessId)
        {
            TeacherId = teacherId;
            SubjectId = subjectId;
            ClassessId = classessId;
        }
    }

    public class Lessons
    {
        public int Id { get; set; }
        public SubjectInfo Subject { get; set; }
        public TeacherInfo Teacher { get; set; }

        public Lessons()
        {
            Subject = new SubjectInfo();
            Teacher = new TeacherInfo();
        }
    }

    public class Timetable
    {
        public ClassInfo? ClassInfo { get; set; }
        public Lessons[,]? Lessons { get; set; }
        public int Score { get; set; }

        public Timetable()
        {
            Lessons = new Lessons[6, 5];
            ClassInfo = new ClassInfo();
            Score = 0;
        }

        public Timetable(ClassInfo? classInfo)
        {
            Lessons = new Lessons[6, 5];
            ClassInfo = classInfo;
            Score = 0;
        }

        public Timetable(ClassInfo? classInfo, Lessons[,]? lessons)
        {
            ClassInfo = classInfo;
            Lessons = lessons;
            Score = 0;
        }
    }

    public class TimetableContainer
    {
        public List<Timetable> Timetables { get; set; }

        public TimetableContainer()
        {
            Timetables = new List<Timetable>();
        }

        public TimetableContainer(List<Timetable> timetables)
        {
            Timetables = timetables;
        }
    }

    public class MaximumLessons
    {
        public int SubjectId { get; set; }
        public int CurentLessons { get; set; }

        public MaximumLessons() { }

        public MaximumLessons(int subjectId, int curentContinousLessons)
        {
            SubjectId = subjectId;
            CurentLessons = curentContinousLessons;
        }
    }


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
