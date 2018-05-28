namespace DBRepository.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;            
        }

        protected override void Seed(Context context)
        {
            Faculty fit;
            Department dep;
            Speciality pin;
            Employee emp;

            context.Employees.AddOrUpdate(e => e.Name,
                emp = new Employee {Name = "������� �.�." },
                new Employee { Name = "������ �.�." },
                new Employee { Name = "������� �.�." },
                new Employee { Name = "������� �.�." },
                new Employee { Name = "������� �.�." },
                new Employee { Name = "���������� �.�." },
                new Employee { Name = "���� �.�." });

            context.Semesters.AddOrUpdate(p => p.Number,
                    new Semester { Number = 1, CountOfWeeks = 17 },
                    new Semester { Number = 2, CountOfWeeks = 18 },
                    new Semester { Number = 3, CountOfWeeks = 17 },
                    new Semester { Number = 4, CountOfWeeks = 17 },
                    new Semester { Number = 5, CountOfWeeks = 17 },
                    new Semester { Number = 6, CountOfWeeks = 18 },
                    new Semester { Number = 7, CountOfWeeks = 15 },
                    new Semester { Number = 8, CountOfWeeks = 15 },
                    new Semester { Number = 9, CountOfWeeks = 12 },
                    new Semester { Number = 10, CountOfWeeks = 12 },
                    new Semester { Number = 11, CountOfWeeks = 11 },
                    new Semester { Number = 12, CountOfWeeks = 5 });
            context.Departments.AddOrUpdate(
             p => p.CodeNumber,
             dep = new Department { CodeNumber = 001, Description = "��" });
            context.Faculties.AddOrUpdate(
              p => p.ShortName,
              fit = new Faculty { ShortName = "���", FullName = "��������� �������������� ����������" },
              new Faculty { ShortName = "���", FullName = "������������������ ���������" },
              new Faculty { ShortName = "��", FullName = "������� ���������" }
            );
            context.Specialities.AddOrUpdate(
              p => p.Name,              
              pin = new Speciality { Name = "���.���", FacultyId = fit.Id }
            );

            context.Discipline.AddOrUpdate(d=>d.Name,
                new Discipline {DepartmentId = dep.Id,Name = "���",TypeOfDiscipline=DisciplineType.SPECIAL,SpecialType = SpecialDisciplineKind.GEK },
                new Discipline { DepartmentId = dep.Id, Name = "��� ���������", TypeOfDiscipline = DisciplineType.SPECIAL, SpecialType = SpecialDisciplineKind.GEK },
                new Discipline { DepartmentId = dep.Id, Name = "���", TypeOfDiscipline = DisciplineType.SPECIAL,SpecialType = SpecialDisciplineKind.GAK },
                new Discipline { DepartmentId = dep.Id, Name = "������������ ���", TypeOfDiscipline = DisciplineType.SPECIAL,SpecialType=SpecialDisciplineKind.GAK_PRED },
                new Discipline { DepartmentId = dep.Id, Name = "����������� ����������", TypeOfDiscipline = DisciplineType.SPECIAL, SpecialType = SpecialDisciplineKind.MAG_RUK },
                new Discipline { DepartmentId = dep.Id, Name = "����������� �����������", TypeOfDiscipline = DisciplineType.SPECIAL, SpecialType = SpecialDisciplineKind.BAK_RUK },
                new Discipline { DepartmentId = dep.Id, Name = "�������������� �������������� �����", TypeOfDiscipline = DisciplineType.SPECIAL, SpecialType = SpecialDisciplineKind.MAG_RETZ},
                new Discipline { DepartmentId = dep.Id, Name = "����������� ��������", TypeOfDiscipline = DisciplineType.SPECIAL, SpecialType = SpecialDisciplineKind.RUK_KAF }
               );
            context.SpecialPositions.AddOrUpdate(
                s => s.Name,
                new SpecialPosition {Name = "���������� ��������",ExecutorId = emp.Id });


            //context.Groups.AddOrUpdate(
            //  p => p.Name,
            //  new Group { Name = "���2014", EntryYear = 2014, CountOfStudents = 13, Qualification = Qualification.Bachelor, SpecialityId = pin.Id, StudyForm = StudyForm.FullTime }
            //);

            //context.Discipline.AddOrUpdate(p => p.Name,
            //    new Discipline { Name = "������� ��������", DepartmentId = dep.Id, IsSpecial = true  },
            //    new Discipline { Name = "���������������� ��������", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "������������� ��������", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "���", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "���", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "������������ ���", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "��������� ������������", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "������ � ���", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "�������������� ���", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "�������������� ��������� ������", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "����������� �����������", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "����������� ����������", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "�������������� ���������", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "����������� ��������", DepartmentId = dep.Id, IsSpecial = true },
            //    new Discipline { Name = "����", DepartmentId = dep.Id, IsSpecial = true });
        }            
    }
}
