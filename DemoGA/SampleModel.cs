using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoGA
{
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

    // Số tiết tối đa / buổi
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
        public int LessonsPerWeek { get; set; } // Số tiết học / tuần
        public int MaximumContinousLessons { get; set; } // Số tiết tối đa liên tiếp
        public List<string>? FixedLessons { get; set; } // List địa chỉ tiết được đánh cố định
        public List<LessonsPerSection>? MinimumLessonsPerSection { get; set; } // Số tiết tối thiểu / buổi (chưa sử dụng)
        public string? SubjectDepartment { get; set; } // Môn học thuộc ban: Tự nhiên / Xã hội (chưa sử dụng)
        public string Section { get; set; } // Tiết này dạy buổi nào

        public SubjectInfo()
        {
            FixedLessons = new List<string>();
            MinimumLessonsPerSection = new List<LessonsPerSection>();
        }

        public SubjectInfo(int id, string? name, int lessonsPerWeek, int maximumContinousLessons, List<string>? fixedLessons, string section)
        {
            Id = id;
            Name = name;
            LessonsPerWeek = lessonsPerWeek;
            MaximumContinousLessons = maximumContinousLessons;
            FixedLessons = fixedLessons;
            Section = section;
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
        public string MainSection { get; set; } // Chính khóa của lớp: buổi sáng / buổi chiều
        public List<string>? OffLessons { get; set; } // List địa chỉ tiết được đánh nghỉ (chưa sử dụng)
        public List<SubjectInfo> Subjects { get; set; }
        public TeacherInfo HeadTeacher { get; set; } // GVCN

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
        public List<AssignedLessonInfo> AssignedLessonInfos { get; set; } // Thông tin các tiết đã được xếp TKB cho giáo viên

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
        public string LessonAddress { get; set; } // Địa chỉ của tiết (mảng 2 chiều, lưu kiểu row_column, vd: 2_0)
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string Section { get; set; } // Tiết được xếp thuộc buổi nào

        public AssignedLessonInfo() { }

        public AssignedLessonInfo(string lessonAddress, int classId, string className, string section)
        {
            LessonAddress = lessonAddress;
            ClassId = classId;
            ClassName = className;
            Section = section;
        }
    }

    // Phân công giảng dạy
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

    // Thời khóa biểu
    public class Timetable
    {
        public ClassInfo? ClassInfo { get; set; }
        public Lessons[,]? Lessons { get; set; } // Danh sách tiết học, mảng 2 chiều
        public int Score { get; set; } // 0 điểm là best
        public List<TrackingError> Err { get; set; } // Tracking lỗi nếu TKB không tính được 0 điểm
        public string Section { get; set; } // TKB thuộc buổi sáng / chiều

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

        public Timetable(ClassInfo? classInfo, Lessons[,]? lessons, string section)
        {
            ClassInfo = classInfo;
            Lessons = lessons;
            Score = 0;
            Err = new List<TrackingError>();
            Section = section;
        }
    }

    // 1 Container chứa danh sách các TKB => dùng để tạo nhiều sample TKB đầu vào để chạy thuật toán
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

    // Dùng để check xem số tiết tối đa được xếp TKB có bằng số tiết tối đa / tuần của môn học hay không
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

    // Tracking lỗi
    public class TrackingError
    {
        public string ClassName { get; set; }
        public string Address { get; set; } // Địa chỉ của tiết bị lỗi, vd: 0_2
        public string Reason { get; set; } // Lý do lỗi
        public int ErrorType { get; set; } // Loại lỗi

        public TrackingError() { }  
    }
}
