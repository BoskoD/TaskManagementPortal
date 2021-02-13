import { Component, OnInit, Input } from '@angular/core';
import {SharedService} from 'src/app/shared.service';


@Component({
  selector: 'app-add-update-project',
  templateUrl: './add-update-project.component.html',
  styleUrls: ['./add-update-project.component.css']
})
export class AddUpdateProjectComponent implements OnInit {

  constructor(private service:SharedService) { }

  @Input() project:any;
  rowKey:string;
  partitionKey:string;
  description: string;
  code: string;


  ngOnInit(): void {
    this.rowKey=this.project.rowKey;
    this.partitionKey=this.project.partitionKey;
    this.description=this.project.description;
    this.code=this.project.code;
  }


  addProject(){
    var val = { rowKey:this.rowKey,
                partitionKey:this.partitionKey,
                description:this.description,
                code:this.code};
    this.service.addProject(val).subscribe(res=>{
      alert("Project added");
    });
  }

  updateProject(){
    var val = { rowKey:this.rowKey,
                partitionKey:this.partitionKey,
                description:this.description,
                code:this.code};
    this.service.updateProject(val).subscribe(res=>{
    alert("Project updated");
    });
  }
}
