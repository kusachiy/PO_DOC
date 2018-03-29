﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Services
{
    public interface IGeneralService
    {
        List<Student> GetAllStudents();
        List<Group> GetAllGroups();
        List<Department> GetAllDepartments();
        List<Discipline> GetAllDisciplines();
        List<DisciplineYear> GetAllDisciplineYears();
        List<Employee> GetAllEmployees();
        List<Faculty> GetAllFaculties();
        List<Speciality> GetAllSpecialities();
        List<Workload> GetAllWorkloads();
        List<Semester> GetAllSemesters();
        List<StudyYear> GetAllStudyYears();

        void AddOrUpdateSemester(Semester sem);
        void DeleteSemester(Semester selectedSemester);
        void DeleteDepartment(Department selectedDepartmnet);
        void AddOrUpdateDepartment(Department dep);
        void DeleteEmployee(Employee selectedEmployee);
        void AddOrUpdateEmployee(Employee emp);
        void AddOrUpdateFaculty(Faculty dep);
        void DeleteFaculty(Faculty selectedFaculty);
        void AddOrUpdateGroup(Group group);
        void AddOrUpdateSpeciality(Speciality speciality);
        void AddOrUpdateDiscipline(Discipline discipline);
        void DeleteGroup(Group selectedGroup);
        void DeleteDiscipline(Discipline selectedDiscipline);
        void AddOrUpdateStudyYear(StudyYear studyYear);
        void AddOrUpdateDisciplineWorkload(DisciplineWorkload workload);     
        void AddOrUpdateWorkload(Workload workload);
        Employee GetLastWorkloadEmployee(Discipline discipline, Group group, StudyYear studyYear);
        void AddOrUpdateDisciplineYear(DisciplineYear disYear);
        void DeleteSpecialPosition(SpecialPosition selectedPosition);
        List<SpecialPosition> GetAllSpecialPostitions();
        void AddOrUpdateSpecialPosition(SpecialPosition sp);
    }
}
