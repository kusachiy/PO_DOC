using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBRepository.Repositories.Interfaces;
using DBRepository;
using DBRepository.Workers.Interfaces;

namespace Diploma.Services
{

    class GeneralService : ServiceBase, IGeneralService
    {
        #region _repositories
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly IDisciplineYearRepository _disciplineYearRepository;
        private readonly IDisciplineWorkloadRepository _disciplineWorkloadRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IGroupsRepository _groupsRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly ISpecialityRepository _specialityRepository;
        private readonly IStudentsRepository _studentsRepository;
        private readonly IStudyYearRepository _studyYearRepository;
        private readonly IWorkloadRepository _workloadRepository;
        private readonly ISpecialPositionRepository _specialPositionRepository;

        #endregion
        #region ctor
        public GeneralService
            (IDbWorker dbWorker,
            IDepartmentRepository departmentRepository, IDisciplineRepository disciplineRepository,
            IDisciplineYearRepository disciplineYearRepository,
            IDisciplineWorkloadRepository disciplineWorkloadRepository,
            IEmployeeRepository employeeRepository, IFacultyRepository facultyRepository,
            IGroupsRepository groupsRepository, ISemesterRepository semesterRepository,
            ISpecialityRepository specialityRepository,
            IStudentsRepository studentsRepository, IStudyYearRepository studyYearRepository,
            IWorkloadRepository workloadRepository, ISpecialPositionRepository specialPositionRepository) : base(dbWorker)
        {
            _departmentRepository = departmentRepository;
            _disciplineRepository = disciplineRepository;
            _disciplineYearRepository = disciplineYearRepository;
            _disciplineWorkloadRepository = disciplineWorkloadRepository;
            _employeeRepository = employeeRepository;
            _facultyRepository = facultyRepository;
            _groupsRepository = groupsRepository;
            _semesterRepository = semesterRepository;
            _specialityRepository = specialityRepository;
            _studentsRepository = studentsRepository;
            _studyYearRepository = studyYearRepository;
            _workloadRepository = workloadRepository;
            _specialPositionRepository = specialPositionRepository;
        }
        #endregion
        #region AddOrUpdate

        public void AddOrUpdateDisciplineWorkload(DisciplineWorkload workload)
        {
            using (var scope = Db.BeginWork())
            {
                var discipline = workload.DisciplineYear;
                if (discipline != null)
                    workload.DisciplineYearId = discipline.Id;
                workload.DisciplineYear = null;

                var group = workload.Group;
                if (group != null)
                    workload.GroupId = group.Id;
                workload.Group = null;

                var semester = workload.Semester;
                if (semester != null)
                    workload.SemesterId = semester.Id;
                workload.Semester = null;

                var studyYear = workload.StudyYear;
                if (studyYear != null)
                    workload.StudyYearId = studyYear.Id;
                workload.StudyYear = null;

                _disciplineWorkloadRepository.AddOrUpdate(workload);
                scope.SaveChanges();

                workload.DisciplineYear = discipline;
                workload.Group = group;
                workload.Semester = semester;
                workload.StudyYear = studyYear;
            }
        }

        public void AddOrUpdateWorkload(Workload workload)
        {
            using (var scope = Db.BeginWork())
            {
                var disciplineWorkload = workload.LocalWorkload;
                if (disciplineWorkload != null)
                    workload.LocalWorkloadId = disciplineWorkload.Id;
                workload.LocalWorkload = null;

                var employee = workload.Employee;
                if (employee != null)
                    workload.EmployeeId = employee.Id;
                workload.Employee = null;                              

                _workloadRepository.AddOrUpdate(workload);
                scope.SaveChanges();

                workload.Employee = employee;
                workload.LocalWorkload = disciplineWorkload;            
            }
        }

        public void AddOrUpdateDepartment(Department dep)
        {
            using (var scope = Db.BeginWork())
            {
                _departmentRepository.AddOrUpdate(dep);
                scope.SaveChanges();
            }
        }

        public void AddOrUpdateDiscipline(Discipline discipline)
        {
            using (var scope = Db.BeginWork())
            {
                var department = discipline.Department;
                if (department != null)
                    discipline.DepartmentId = department.Id;
                discipline.Department = null;
                _disciplineRepository.AddOrUpdate(discipline);
                scope.SaveChanges();
                discipline.Department = department;
            }
        }
        public void AddOrUpdateDisciplineYear(DisciplineYear disYear)
        {
            using (var scope = Db.BeginWork())
            {
                var discipline = disYear.Discipline;
                if (discipline != null)
                    disYear.DisciplineId = discipline.Id;
                disYear.Discipline = null;

                _disciplineYearRepository.AddOrUpdate(disYear);
                scope.SaveChanges();

                disYear.Discipline = discipline;
            }
        }
        public void AddOrUpdateEmployee(Employee emp)
        {
            using (var scope = Db.BeginWork())
            {
                _employeeRepository.AddOrUpdate(emp);
                scope.SaveChanges();
            }
        }

        public void AddOrUpdateFaculty(Faculty fac)
        {
            using (var scope = Db.BeginWork())
            {
                _facultyRepository.AddOrUpdate(fac);
                scope.SaveChanges();
            }
        }

        public void AddOrUpdateGroup(Group group)
        {
            using (var scope = Db.BeginWork())
            {
                var speciality = group.Speciality;
                if (speciality != null)
                    group.SpecialityId = speciality.Id;
                group.Speciality = null;
                _groupsRepository.AddOrUpdate(group);
                scope.SaveChanges();
                group.Speciality = speciality;
            }
        }

        public void AddOrUpdateSemester(Semester sem)
        {
            using (var scope = Db.BeginWork())
            {
                _semesterRepository.AddOrUpdate(sem);
                scope.SaveChanges();
            }
        }

        public void AddOrUpdateSpeciality(Speciality speciality)
        {
            using (var scope = Db.BeginWork())
            {
                var faculty = speciality.Faculty;
                if (faculty != null)
                    speciality.FacultyId = faculty.Id;
                speciality.Faculty = null;
                _specialityRepository.AddOrUpdate(speciality);
                scope.SaveChanges();
                speciality.Faculty = faculty;
            }
        }

        public void AddOrUpdateStudyYear(StudyYear studyYear)
        {
            using (var scope = Db.BeginWork())
            {
                _studyYearRepository.AddOrUpdate(studyYear);
                scope.SaveChanges();
            }
        }

        public void AddWorkload(Workload workload)
        {
            using (var scope = Db.BeginWork())
            {
                _workloadRepository.Add(workload);
                scope.SaveChanges();
            }
        }

        public void AddOrUpdateSpecialPosition(SpecialPosition sp)
        {
            using (var scope = Db.BeginWork())
            {
                var emp = sp.Executor;
                if (emp != null)
                    sp.ExecutorId = emp.Id;
                sp.Executor = null;
                _specialPositionRepository.AddOrUpdate(sp);
                scope.SaveChanges();
                sp.Executor = emp;
            }
        }




        #endregion
        #region Delete
        public void DeleteDepartment(Department dep)
        {
            using (var scope = Db.BeginWork())
            {
                var baseSem = _departmentRepository.Get(u => u.Id == dep.Id);
                if (baseSem != null)
                {
                    _departmentRepository.Delete(baseSem);
                    scope.SaveChanges();
                }
            }
        }

        public void DeleteDiscipline(Discipline selectedDiscipline)
        {
            using (var scope = Db.BeginWork())
            {
                var baseEntity = _disciplineRepository.Get(u => u.Id == selectedDiscipline.Id);
                if (baseEntity != null)
                {
                    _disciplineRepository.Delete(baseEntity);
                    scope.SaveChanges();
                }
            }
        }

        public void DeleteEmployee(Employee emp)
        {
            using (var scope = Db.BeginWork())
            {               
                var baseEmp = _employeeRepository.Get(u => u.Id == emp.Id);
                if (baseEmp != null)
                {
                    var workloads = GetAllEmloyeeWorkloads(baseEmp);
                    foreach (var w in workloads)
                        w.EmployeeId = null;
                    _employeeRepository.Delete(baseEmp);
                    scope.SaveChanges();
                }
            }
        }

        public void DeleteFaculty(Faculty faculty)
        {
            using (var scope = Db.BeginWork())
            {
                var baseFaculty = _facultyRepository.Get(u => u.Id == faculty.Id);
                if (baseFaculty != null)
                {
                    _facultyRepository.Delete(baseFaculty);
                    scope.SaveChanges();
                }
            }
        }

        public void DeleteGroup(Group selectedGroup)
        {
            using (var scope = Db.BeginWork())
            {
                var baseGroup = _groupsRepository.Get(u => u.Id == selectedGroup.Id);
                if (baseGroup != null)
                {
                    _groupsRepository.Delete(baseGroup);
                    scope.SaveChanges();
                }
            }
        }

        public void DeleteSemester(Semester sem)
        {
            using (var scope = Db.BeginWork())
            {
                var baseSem = _semesterRepository.Get(u => u.Id == sem.Id);
                if (baseSem != null)
                {
                    _semesterRepository.Delete(baseSem);
                    scope.SaveChanges();
                }
            }
        }

        public void DeleteSpecialPosition(SpecialPosition selectedPosition)
        {
            using (var scope = Db.BeginWork())
            {
                var baseEntity = _specialPositionRepository.Get(u => u.Id == selectedPosition.Id);
                if (baseEntity != null)
                {
                    _specialPositionRepository.Delete(baseEntity);
                    scope.SaveChanges();
                }
            }
        }
        public void DeleteWorkload(Workload workload)
        {
            using (var scope = Db.BeginWork())
            {
                var baseEntity = _workloadRepository.Get(u => u.Id == workload.Id);
                if (baseEntity != null)
                {
                    _workloadRepository.Delete(baseEntity);
                    scope.SaveChanges();
                }
            }
        }
        #endregion
        #region GetAll()
        public List<Department> GetAllDepartments()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _departmentRepository.GetAll();
            }
        }

        public List<Discipline> GetAllDisciplines()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _disciplineRepository.GetAll(d => d.Department);
            }
        }

        public List<DisciplineYear> GetAllDisciplineYears()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _disciplineYearRepository.GetAll();
            }
        }
        public List<Employee> GetAllEmployees()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _employeeRepository.GetAll();
            }
        }

        public List<Faculty> GetAllFaculties()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _facultyRepository.GetAll();
            }
        }

        public List<Group> GetAllGroups()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _groupsRepository.GetAll(g => g.Speciality);
            }
        }

        public List<Semester> GetAllSemesters()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _semesterRepository.GetAll();
            }
        }

        public List<Speciality> GetAllSpecialities()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _specialityRepository.GetAll(s => s.Faculty);
            }
        }

        public List<Student> GetAllStudents()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _studentsRepository.GetAll();
            }
        }

        public List<StudyYear> GetAllStudyYears()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _studyYearRepository.GetAll();
            }
        }

        public List<Workload> GetAllWorkloads()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _workloadRepository.GetAll(w => w.LocalWorkload.DisciplineYear.Discipline, w => w.LocalWorkload.Group, w => w.LocalWorkload.Semester, w => w.Employee);
            }
        }

        public List<SpecialPosition> GetAllSpecialPostitions()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _specialPositionRepository.GetAll(s => s.Executor);
            }
        }
        #endregion


        public Guid? GetLastWorkloadEmployeeId(Discipline discipline, Group group, StudyYear studyYear)
        {
            using (Db.BeginReadOnlyWork())
            {
                if (_workloadRepository.Count() == 0)
                    return null;
                var workload = _workloadRepository.Get(
                    w => w.LocalWorkload.DisciplineYear.DisciplineId == discipline.Id
                    //&& w.LocalWorkload.Group.SpecialityId == group.SpecialityId
                    && w.LocalWorkload.StudyYear.Year == studyYear.Year - 1);
                return workload?.EmployeeId;
            }
        }
        public List<Workload> GetAllWorkloadsByYear(StudyYear selectedStudyYear)
        {
            using (Db.BeginReadOnlyWork())
            {
                return _workloadRepository.GetMany(w => w.LocalWorkload.StudyYearId == selectedStudyYear.Id, w => w.LocalWorkload.DisciplineYear.Discipline, w => w.LocalWorkload.Group, w => w.LocalWorkload.Semester, w => w.Employee,  w => w.LocalWorkload.Group.Speciality.Faculty);
            }
        }
        public void UpdateWorkload(Workload workload)
        {
            using (var scope = Db.BeginWork())
            {
                if (workload.Employee == null)
                    return;
                var baseEntity = _workloadRepository.Get(w => w.Id == workload.Id);
                baseEntity.EmployeeId = workload.Employee.Id;
                _workloadRepository.Update(baseEntity);
                scope.SaveChanges();
            }
        }
        public List<Workload> GetAllEmloyeeWorkloads(Employee employee)
        {
            using (Db.BeginReadOnlyWork())
            {
                return _workloadRepository.GetMany(w => w.EmployeeId == employee.Id);//фильтр
                   //выборка
            }
        }
        public List<DisciplineWorkload> GetAllEmloyeeWorkloadsByYear(Employee employee, int year)
        {
            using (Db.BeginReadOnlyWork())
            {
                return _workloadRepository.GetMany(w => w.EmployeeId == employee.Id && w.LocalWorkload.StudyYear.Year == year,//фильтр
                    w => w.LocalWorkload, w => w.LocalWorkload.DisciplineYear, w => w.LocalWorkload.DisciplineYear.Discipline, w => w.LocalWorkload.Group, w => w.LocalWorkload.Group.Speciality, w => w.LocalWorkload.Group.Speciality.Faculty, w => w.LocalWorkload.Semester)//включения
                    .Select(w => w.LocalWorkload).ToList();//выборка
            }
        }
        public List<DisciplineWorkload> GetAllEmloyeeWorkloadsByYearAutumm(Employee employee, int year)
        {
            using (Db.BeginReadOnlyWork())
            {
                return GetAllEmloyeeWorkloadsByYear(employee, year).Where(e => e.Semester?.Type == SemesterType.Autumm).ToList();
            }
        }
        public List<DisciplineWorkload> GetAllEmloyeeWorkloadsByYearSpring(Employee employee, int year)
        {
            using (Db.BeginReadOnlyWork())
            {
                return GetAllEmloyeeWorkloadsByYear(employee, year).Where(e => e.Semester?.Type == SemesterType.Spring).ToList();
            }
        }

        public List<DisciplineWorkload> GetAllDisciplineWorkloadsByYearAndSemesterType(int year, SemesterType semester)
        {
            using (Db.BeginReadOnlyWork())
            {
                return _disciplineWorkloadRepository
                    .GetMany(w => w.StudyYear.Year == year,
                    w=>w.Group,w=>w.DisciplineYear,w=>w.DisciplineYear.Discipline,w=>w.Semester,w => w.Group.Speciality.Faculty,w=>w.Group.Speciality)
                    .Where(w => w.Semester?.Type == semester).ToList();
            }
        }

        public List<DisciplineWorkload> GetAllDisciplineWorkloadsByYear(StudyYear year)
        {
            using (Db.BeginReadOnlyWork())
            {
                return _disciplineWorkloadRepository
                    .GetMany(w => w.StudyYearId == year.Id,
                    w => w.Group, w => w.DisciplineYear, w => w.DisciplineYear.Discipline, w => w.Semester, w => w.Group.Speciality.Faculty, w => w.Group.Speciality).ToList();
            }
        }

        public List<Workload> GetAllWorkloadsByLocalWorkload(Guid id)
        {
            using (Db.BeginReadOnlyWork())
            {
                return _workloadRepository.GetMany(w => w.LocalWorkload.Id == id, w => w.LocalWorkload.DisciplineYear.Discipline, w => w.LocalWorkload.Group, w => w.LocalWorkload.Semester, w => w.Employee, w => w.LocalWorkload.Group.Speciality.Faculty);
            }
        }

        public List<DisciplineWorkload> GetAllDisciplineWorkloadsByYearAndSemester(StudyYear selectedStudyYear, Semester selectedSemester)
        {
            using (Db.BeginReadOnlyWork())
            {
                return _disciplineWorkloadRepository
                    .GetMany(w => w.StudyYearId==selectedStudyYear.Id&&w.SemesterId==selectedSemester.Id,
                    w => w.Group, w => w.DisciplineYear, w => w.DisciplineYear.Discipline).ToList();
            }
        }

        public List<Discipline> GetAllSpecialDisciplines()
        {
            using (Db.BeginReadOnlyWork())
            {
                return _disciplineRepository.GetMany(d=>d.TypeOfDiscipline == DisciplineType.SPECIAL);
            }
        }

        public void SaveDisciplineWorkloads(List<DisciplineWorkload> disciplineWorkloads)
        {
            foreach (var d in disciplineWorkloads)
            {
                d.DisciplineYearId = d.DisciplineYear.Id;
                AddOrUpdateDisciplineYear(d.DisciplineYear);
                AddOrUpdateDisciplineWorkload(d);
            }
        }
    }
}
