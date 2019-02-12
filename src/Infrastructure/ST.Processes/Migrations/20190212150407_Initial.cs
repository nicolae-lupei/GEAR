﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ST.Procesess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Processes");

            migrationBuilder.CreateTable(
                name: "ProcessSchemas",
                schema: "Processes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Diagram = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    Synchronized = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    InitialSchemaId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessSchemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessSchemas_ProcessSchemas_InitialSchemaId",
                        column: x => x.InitialSchemaId,
                        principalSchema: "Processes",
                        principalTable: "ProcessSchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                schema: "Processes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ProcessSchemaId = table.Column<Guid>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    IntitialProcessId = table.Column<Guid>(nullable: true),
                    ProcessSettings = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processes_Processes_IntitialProcessId",
                        column: x => x.IntitialProcessId,
                        principalSchema: "Processes",
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Processes_ProcessSchemas_ProcessSchemaId",
                        column: x => x.ProcessSchemaId,
                        principalSchema: "Processes",
                        principalTable: "ProcessSchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessInstances",
                schema: "Processes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StartedBy = table.Column<Guid>(nullable: false),
                    ProcessId = table.Column<Guid>(nullable: false),
                    InstanceState = table.Column<int>(nullable: false),
                    Progress = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessInstances_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalSchema: "Processes",
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessTransitions",
                schema: "Processes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ProcessTransitionId = table.Column<Guid>(nullable: true),
                    TransitionType = table.Column<int>(nullable: false),
                    ProcessId = table.Column<Guid>(nullable: false),
                    TransitionSettings = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTransitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessTransitions_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalSchema: "Processes",
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessTransitions_ProcessTransitions_ProcessTransitionId",
                        column: x => x.ProcessTransitionId,
                        principalSchema: "Processes",
                        principalTable: "ProcessTransitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessInstanceHistories",
                schema: "Processes",
                columns: table => new
                {
                    ProcessInstanceId = table.Column<Guid>(nullable: false),
                    ProcessTransitionId = table.Column<Guid>(nullable: false),
                    TransitionState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessInstanceHistories", x => new { x.ProcessTransitionId, x.ProcessInstanceId });
                    table.ForeignKey(
                        name: "FK_ProcessInstanceHistories_ProcessInstances_ProcessInstanceId",
                        column: x => x.ProcessInstanceId,
                        principalSchema: "Processes",
                        principalTable: "ProcessInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessInstanceHistories_ProcessTransitions_ProcessTransitionId",
                        column: x => x.ProcessTransitionId,
                        principalSchema: "Processes",
                        principalTable: "ProcessTransitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessTasks",
                schema: "Processes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProcessTaskState = table.Column<int>(nullable: false),
                    Life = table.Column<int>(nullable: false),
                    Assigned = table.Column<Guid>(nullable: true),
                    ProcessTransitionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessTasks_ProcessTransitions_ProcessTransitionId",
                        column: x => x.ProcessTransitionId,
                        principalSchema: "Processes",
                        principalTable: "ProcessTransitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransitionActors",
                schema: "Processes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    ProcessTransitionId = table.Column<Guid>(nullable: false),
                    ActorSettings = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitionActors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransitionActors_ProcessTransitions_ProcessTransitionId",
                        column: x => x.ProcessTransitionId,
                        principalSchema: "Processes",
                        principalTable: "ProcessTransitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProcessTasks",
                schema: "Processes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProcessTaskId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProcessTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProcessTasks_ProcessTasks_ProcessTaskId",
                        column: x => x.ProcessTaskId,
                        principalSchema: "Processes",
                        principalTable: "ProcessTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processes_IntitialProcessId",
                schema: "Processes",
                table: "Processes",
                column: "IntitialProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_ProcessSchemaId",
                schema: "Processes",
                table: "Processes",
                column: "ProcessSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessInstanceHistories_ProcessInstanceId",
                schema: "Processes",
                table: "ProcessInstanceHistories",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessInstances_ProcessId",
                schema: "Processes",
                table: "ProcessInstances",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessSchemas_InitialSchemaId",
                schema: "Processes",
                table: "ProcessSchemas",
                column: "InitialSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTasks_ProcessTransitionId",
                schema: "Processes",
                table: "ProcessTasks",
                column: "ProcessTransitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTransitions_ProcessId",
                schema: "Processes",
                table: "ProcessTransitions",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTransitions_ProcessTransitionId",
                schema: "Processes",
                table: "ProcessTransitions",
                column: "ProcessTransitionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitionActors_ProcessTransitionId",
                schema: "Processes",
                table: "TransitionActors",
                column: "ProcessTransitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProcessTasks_ProcessTaskId",
                schema: "Processes",
                table: "UserProcessTasks",
                column: "ProcessTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessInstanceHistories",
                schema: "Processes");

            migrationBuilder.DropTable(
                name: "TransitionActors",
                schema: "Processes");

            migrationBuilder.DropTable(
                name: "UserProcessTasks",
                schema: "Processes");

            migrationBuilder.DropTable(
                name: "ProcessInstances",
                schema: "Processes");

            migrationBuilder.DropTable(
                name: "ProcessTasks",
                schema: "Processes");

            migrationBuilder.DropTable(
                name: "ProcessTransitions",
                schema: "Processes");

            migrationBuilder.DropTable(
                name: "Processes",
                schema: "Processes");

            migrationBuilder.DropTable(
                name: "ProcessSchemas",
                schema: "Processes");
        }
    }
}
