using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoGA
{
    [Serializable]
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
    [Serializable]
    public class LessonsPerSection
    {
        public string Section { get; set; }
        public int NumOfLessons { get; set; }

        public LessonsPerSection()
        {
        }

        public LessonsPerSection(string section, int numOfLessons)
        {
            Section = section;
            NumOfLessons = numOfLessons;
        }
    }

    [Serializable]
    public class SubjectInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int LessonsPerWeek { get; set; } // Số tiết học / tuần
        public int MaximumContinousLessons { get; set; } // Số tiết tối đa liên tiếp
        public List<LessonAddress>? FixedLessons { get; set; } // List địa chỉ tiết được đánh cố định
        public List<LessonsPerSection> LessonsPerSections { get; set; }
        public string? SubjectDepartment { get; set; } // Môn học thuộc ban: Tự nhiên / Xã hội (chưa sử dụng)
        public string Section { get; set; } // Tiết này dạy buổi nào
        public List<LessonsPerSectionDetail> LessonsPerSectionDetails { get; set; }

        public SubjectInfo()
        {
            FixedLessons = new List<LessonAddress>();
            LessonsPerSections = new List<LessonsPerSection>();
            LessonsPerSectionDetails = new List<LessonsPerSectionDetail>();
        }

        public SubjectInfo(int id, string? name, int lessonsPerWeek, int maximumContinousLessons, string section)
        {
            Id = id;
            Name = name;
            LessonsPerWeek = lessonsPerWeek;
            MaximumContinousLessons = maximumContinousLessons;
            Section = section;
            FixedLessons = new List<LessonAddress>();
            LessonsPerSections = new List<LessonsPerSection>();
            LessonsPerSectionDetails = new List<LessonsPerSectionDetail>();
        }
    }

    [Serializable]
    public class ClassInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MainSection { get; set; } // Chính khóa của lớp: buổi sáng / buổi chiều
        public List<SubjectInfo> Subjects { get; set; }
        public TeacherInfo HeadTeacher { get; set; } // GVCN
        public List<SectionDetailInfo> SectionDetailInfos { get; set; }

        public ClassInfo()
        {
            Subjects = new List<SubjectInfo>();
            HeadTeacher = new TeacherInfo();
            SectionDetailInfos = new List<SectionDetailInfo>();
        }
    }

    [Serializable]
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

    [Serializable]
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

    [Serializable]
    public class AssignedLessonInfo
    {
        public LessonAddress Address { get; set; } // Địa chỉ của tiết (mảng 2 chiều)
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string Section { get; set; } // Tiết được xếp thuộc buổi nào

        public AssignedLessonInfo() { }

        public AssignedLessonInfo(LessonAddress lessonAddress, int classId, string className, string section)
        {
            Address = lessonAddress;
            ClassId = classId;
            ClassName = className;
            Section = section;
        }
    }

    // Phân công giảng dạy
    [Serializable]
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

    [Serializable]
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

        // Use for create sample test
        public Lessons(int id, SubjectInfo subject)
        {
            Id = id;
            Subject = subject;
            Teacher = new TeacherInfo();
            IsLock = 0;
        }
        // Use for create sample test
        public Lessons(int id, SubjectInfo subject, int isLock)
        {
            Id = id;
            Subject = subject;
            Teacher = new TeacherInfo();
            IsLock = isLock;
        }   
    }

    // Thời khóa biểu
    [Serializable]
    public class Timetable
    {
        public ClassInfo? ClassInfo { get; set; }
        public Lessons[,]? Lessons { get; set; } // Danh sách tiết học, mảng 2 chiều
        public int Score { get; set; } // 0 điểm là best
        public List<TrackingError> Err { get; set; } // Tracking lỗi nếu TKB không tính được 0 điểm
        public string Section { get; set; } // TKB thuộc buổi sáng / chiều
        public List<AssignedDuplicateLessonsInfo> ADL { get; set; }

        public Timetable()
        {
            Lessons = new Lessons[6, 5];
            ClassInfo = new ClassInfo();
            Score = 0;
            Err = new List<TrackingError>();
            ADL = new List<AssignedDuplicateLessonsInfo>();
        }

        public Timetable(ClassInfo? classInfo)
        {
            Lessons = new Lessons[6, 5];
            ClassInfo = classInfo;
            Score = 0;
            Err = new List<TrackingError>();
            ADL = new List<AssignedDuplicateLessonsInfo>();
        }

        public Timetable(ClassInfo? classInfo, Lessons[,]? lessons, string section)
        {
            ClassInfo = classInfo;
            Lessons = lessons;
            Score = 0;
            Err = new List<TrackingError>();
            ADL = new List<AssignedDuplicateLessonsInfo>();
            Section = section;
        }
    }

    // 1 Container chứa danh sách các TKB => dùng để tạo nhiều sample TKB đầu vào để chạy thuật toán
    [Serializable]
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
    [Serializable]
    public class MaximumLessons
    {
        public int SubjectId { get; set; }
        public int CurentLessonsCount { get; set; }

        public MaximumLessons() { }

        public MaximumLessons(int subjectId, int curentContinousLessons)
        {
            SubjectId = subjectId;
            CurentLessonsCount = curentContinousLessons;
        }
    }

    // Tracking lỗi
    [Serializable]
    public class TrackingError
    {
        public string ClassName { get; set; }
        public LessonAddress Address { get; set; } // Địa chỉ của tiết bị lỗi
        public string Reason { get; set; } // Lý do lỗi
        public int ErrorType { get; set; } // Loại lỗi

        public TrackingError() { }
    }

    // Tracking tiết học đã xếp => đánh điểm tiết đúp
    [Serializable]
    public class TrackingAssignedLessons
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int TotalLessons { get; set; }
        public List<LessonAddress> LessonsAddress { get; set; }

        public TrackingAssignedLessons()
        {
            LessonsAddress = new List<LessonAddress>();
        }
    }

    [Serializable]
    public class AssignedDuplicateLessonsInfo
    {
        public int SubjectId { get; set; }
        public int CurrentPair { get; set; }
        public int MaximumPair { get; set; }
        public List<LessonAddress> SingleAddress { get; set; } = new List<LessonAddress>();

        public AssignedDuplicateLessonsInfo() {
            SingleAddress = new List<LessonAddress>();
        }
    }

    [Serializable]
    public class LessonAddress
    {
        public int row { get; set; }
        public int col { get; set; }
        
        public LessonAddress(int r, int c)
        {
            row = r;
            col = c;
        }
    }

    [Serializable]
    public class ReferenceLessons
    {
        public LessonAddress Address { get; set; }
        public LessonAddress? RefAddress { get; set; }
        public int SubjectId { get; set; }

        public ReferenceLessons(LessonAddress address, LessonAddress refAddress, int subjectId)
        {
            Address = address;
            RefAddress = refAddress;
            SubjectId = subjectId;
        }
    }

    public class SectionDetailInfo
    {
        public string Section { get; set; }
        public int DayOfWeek { get; set; }
        public int NumOfStudyLessons { get; set; }
        public List<LessonAddress> OffLessons { get; set; }

        public SectionDetailInfo()
        {
            OffLessons = new List<LessonAddress>();
        }

        public SectionDetailInfo(string section, int dayOfWeek, int numOfStudyLessons, List<LessonAddress> offLessons)
        {
            Section = section;
            DayOfWeek = dayOfWeek;
            NumOfStudyLessons = numOfStudyLessons;
            OffLessons = offLessons;
        }
    }

    public class LessonsPerSectionDetail
    {
        public int NumOfContinousLessons { get; set; }
        public int NumOfSections { get; set; }
        
        public LessonsPerSectionDetail() {}

        public LessonsPerSectionDetail(int numOfContinousLessons, int numOfSections)
        {
            NumOfContinousLessons = numOfContinousLessons;
            NumOfSections = numOfSections;
        }
    }

    enum CustomDayOfWeek
    {
        MONDAY,
        TUESDAY,
        WEDNESDAY,
        THURSDAY,
        FRIDAY,
        SATURDAY,
        SUNDAY
    }
}
