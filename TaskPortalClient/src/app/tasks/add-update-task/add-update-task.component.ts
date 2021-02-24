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
  partitionKey:string;
  name:string;
  description: string;
  isComplete: boolean;

  ProjectsList:any=[];

  ngOnInit(): void {
    this.loadProjectsList();
  }

  loadProjectsList(){
    this.service.readAllProjectNames().subscribe((data:any)=>{
      this.ProjectsList=data;

      this.name=this.task.name;
      this.partitionKey=data.id;
      this.description=this.task.description;
      this.isComplete=this.task.isComplete;

      console.log(data);

    });
  }

  addTask(){
    var val = { partitionKey:this.partitionKey,
                name:this.name,
                description:this.description};
    this.service.addTask(val).subscribe(res=>{
      console.log(res);
      alert("New task created");
    });
  }

  updateTask(){
    var val = { partitionKey:this.partitionKey,
                name:this.name,
                description:this.description,
                isComplete:this.isComplete};
    this.service.updateTask(val).subscribe(res=>{
      console.log(res);
    alert("Task updated");
    });
  }
}
