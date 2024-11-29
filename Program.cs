using EfForeignKeyIssue.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

// This was a database first project:

// 1.) Create tables below manually on database
// 2.) Make sure to update connection string in MyDbContext

//CREATE SCHEMA Scheduler;

//CREATE TABLE Scheduler.schedule (
//	pk_id int IDENTITY(0,1) NOT NULL,
//    description varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT '' NOT NULL,
//    CONSTRAINT schedules_pk PRIMARY KEY (pk_id)
//);

//CREATE TABLE Scheduler.tasks (
//	pk_id int IDENTITY(0,1) NOT NULL,
//    description varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Integration task' NOT NULL,
//    CONSTRAINT tasks_pk PRIMARY KEY (pk_id)
//);

//CREATE TABLE Scheduler.schedule_task_mapping (
//	schedule_id int NOT NULL,
//    group_id int NOT NULL,
//    CONSTRAINT schedule_task_mapping_pk PRIMARY KEY (schedule_id, group_id)
//);

//ALTER TABLE Scheduler.schedule_task_mapping ADD CONSTRAINT schedule_task_mapping_schedule_FK FOREIGN KEY (schedule_id) REFERENCES Scheduler.schedule(pk_id) ON DELETE CASCADE ON UPDATE CASCADE;
//ALTER TABLE Scheduler.schedule_task_mapping ADD CONSTRAINT schedule_task_mapping_task_FK FOREIGN KEY (group_id) REFERENCES Scheduler.tasks(pk_id) ON DELETE CASCADE ON UPDATE CASCADE;




Console.WriteLine("Creating entities...");

using (var dbContext = new MyDbContext())
{
    var task1 = new DbTask()
    {
        Description = "First task (ID 0)"
    };

    var schedule1 = new Schedule()
    {
        Description = "First schedule (ID 0)"
    };

    var task2 = new DbTask()
    {
        Description = "Second task (ID 1)"
    };

    var schedule2 = new Schedule()
    {
        Description = "Second schedule (ID 1)"
    };

    dbContext.Tasks.Add(task1);
    dbContext.Schedules.Add(schedule1);
    dbContext.Tasks.Add(task2);
    dbContext.Schedules.Add(schedule2);

    dbContext.SaveChanges();
}

Console.WriteLine("Linking entities...");
using (var dbContext = new MyDbContext())
{
    var task1 = dbContext.Tasks
        .Include(x => x.Schedules)
        .Where(x => x.PkId == 0).FirstOrDefault();
    var schedule1 = dbContext.Schedules
        .Where(x => x.PkId == 0).FirstOrDefault();

    var task2 = dbContext.Tasks
        .Include(x => x.Schedules)
        .Where(x => x.PkId == 1).FirstOrDefault();
    var schedule2 = dbContext.Schedules
        .Where(x => x.PkId == 1).FirstOrDefault();



    task2.Schedules.Add(schedule2); // Works
    dbContext.SaveChanges();

    task1.Schedules.Add(schedule1); // Fails
    dbContext.SaveChanges();
}

Console.WriteLine("Done.");
