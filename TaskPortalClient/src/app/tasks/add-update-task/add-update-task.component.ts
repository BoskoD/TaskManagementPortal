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
  rowKey:string;
  name:string;
  description: string;
  isComplete: string;

  ProjectsList:any=[];

  ngOnInit(): void {
    this.loadProjectsList();
  }

  loadProjectsList(){
    this.service.readAllProjectNames().subscribe((data:any)=>{
      this.ProjectsList=data;

      this.rowKey=this.task.rowKey;
      this.name=this.task.name;
      this.partitionKey=this.task.partitionKey;
      this.description=this.task.description;
      this.isComplete=this.task.isComplete;

      console.log(data);

    });
  }

  addTask(){
    var val = { partitionKey:this.partitionKey,
                rowKey:this.rowKey,
                name:this.name,
                description:this.description,
                isComplete:this.isComplete};
    this.service.addTask(val).subscribe(res=>{
      alert("New task created");
    });
  }

  updateTask(){
    var val = { partitionKey:this.partitionKey,
                rowKey:this.rowKey,
                name:this.name,
                description:this.description,
                isComplete:this.isComplete};
    this.service.updateTask(val).subscribe(res=>{
    alert("Task updated");
    });
  }
}
