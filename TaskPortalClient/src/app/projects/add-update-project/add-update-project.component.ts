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
  id:string;
  name:string;
  description: string;
  code: string;


  ngOnInit(): void {
    this.id=this.project.id;
    this.name=this.project.name;
    this.description=this.project.description;
    this.code=this.project.code;
  }


  addProject(){
    var val = { id:this.id,
                name:this.name,
                description:this.description,
                code:this.code};
    this.service.addProject(val).subscribe(res=>{
      console.log(res);
      alert("Project added");
    });
  }

  updateProject(){
    var val = { id:this.id,
                name:this.name,
                description:this.description,
                code:this.code};
                console.log(val);
    this.service.updateProject(val).subscribe(res=>{
        console.log(res);
        alert("Project updated");
      });
    };
  }

