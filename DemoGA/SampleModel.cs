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
        public List<AssignedLessonInfo> AssignedLessonInfos { get; set; }

        public TeacherAssignedLessonsInfo()
        {
            AssignedLessonInfos = new List<AssignedLessonInfo>();
        }

        public TeacherAssignedLessonsInfo(int teacherId, List<AssignedLessonInfo> assignedLessonInfo)
        {
            TeacherId = teacherId;
            AssignedLessonInfos = assignedLessonInfo;
        }

        public TeacherAssignedLessonsInfo(TeacherAssignedLessonsInfo data)
        {
            TeacherId = data.TeacherId;
            AssignedLessonInfos = data.AssignedLessonInfos;
        }
    }

    public class AssignedLessonInfo
    {
        public string LessonAddress { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public AssignedLessonInfo() { }

        public AssignedLessonInfo(string lessonAddress, int classId, string className)
        {
            LessonAddress = lessonAddress;
            ClassId = classId;
            ClassName = className;
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
        public int IsLock { get; set; }

        public Lessons()
        {
            Subject = new SubjectInfo();
            Teacher = new TeacherInfo();
            IsLock = 0;
        }
    }

    public class Timetable
    {
        public ClassInfo? ClassInfo { get; set; }
        public Lessons[,]? Lessons { get; set; }
        public int Score { get; set; }
        public List<TrackingError> Err { get; set; }

        public Timetable()
        {
            Lessons = new Lessons[6, 5];
            ClassInfo = new ClassInfo();
            Score = 0;
            Err = new List<TrackingError>();
        }

        public Timetable(ClassInfo? classInfo)
        {
            Lessons = new Lessons[6, 5];
            ClassInfo = classInfo;
            Score = 0;
            Err = new List<TrackingError>();
        }

        public Timetable(ClassInfo? classInfo, Lessons[,]? lessons)
        {
            ClassInfo = classInfo;
            Lessons = lessons;
            Score = 0;
            Err = new List<TrackingError>();
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

    public class TrackingError
    {
        public string ClassName { get; set; }
        public string Address { get; set; }
        public string Reason { get; set; }
        public int ErrorType { get; set; }

        public TrackingError() { }  
    }
}
