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
  projectId:string;
  id: string;
  name: string;
  description: string;
  isComplete: string;

  ProjectsList:any;


  ngOnInit(): void {
    this.loadProjectsList();
  }

  loadProjectsList(){
    this.service.getProjectsList().subscribe((data:any)=>{
      this.ProjectsList=data;

      this.id = this.task.id;
      this.name=this.task.name;
      this.projectId=data.id
      this.description=this.task.description;
      this.isComplete=this.task.isComplete;

      console.log(data);
    });
  }

  addTask(){
    var val = { 
                id: this.id,
                projectId: this.projectId,
                name: this.name,
                description: this.description};
    this.service.addTask(val).subscribe(res=>{
      console.log(res);
      alert("New task created");
    });
  }


  updateTask(){
    var val = { id: this.id,
                projectId: this.projectId,
                name: this.name,
                description: this.description,
                isComplete:this.isComplete};
                console.log(val);
    this.service.updateTask(val).subscribe(res=>{console.log(res);
        alert("Task updated");
      }, error => {console.log(error);
    });
  }
}