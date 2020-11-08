import { Component, OnInit, Input } from '@angular/core';
import {SharedService} from 'src/app/shared.service';

@Component({
  selector: 'app-add-update-task',
  templateUrl: './add-update-task.component.html',
  styleUrls: ['./add-update-task.component.css']
})
export class AddUpdateTaskComponent implements OnInit {

  constructor(private service:SharedService) { }

  @Input() task:any;
  Project:string;
  Id:string;
  Name:string;
  Description: string;
  IsComplete: string;

  ProjectsList:any=[];

  ngOnInit(): void {
    this.loadProjectsList();
  }

  loadProjectsList(){
    this.service.getAllProjectNames().subscribe((data:any)=>{
      this.ProjectsList=data;

      this.Id=this.task.Id;
      this.Name=this.task.Name;
      this.Project=this.task.Project;
      this.Description=this.task.Description;
      this.IsComplete=this.task.IsComplete;

    
    });
  }

  addTask(){
    var val = { Project:this.Project,
                Id:this.Id,
                Name:this.Name,
                Description:this.Description,
                IsComplete:this.IsComplete};
    this.service.addTask(val).subscribe(res=>{
      alert("New task created");
    });
  }

  updateTask(){
    var val = { Project:this.Project,
                Id:this.Id,
                Name:this.Name,
                Description:this.Description,
                IsComplete:this.IsComplete};
    this.service.updateTask(val).subscribe(res=>{
    alert("Task updated");
    });
  }
}
