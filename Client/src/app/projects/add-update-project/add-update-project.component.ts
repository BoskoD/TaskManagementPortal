import { Component, OnInit, Input } from '@angular/core';
import {SharedService} from 'src/app/shared.service';


@Component({
  selector: 'app-add-update-project',
  templateUrl: './add-update-project.component.html',
  styleUrls: ['./add-update-project.component.css']
})
export class AddUpdateProjectComponent implements OnInit {

  constructor(private service:SharedService) { }

  @Input() proj:any;
  Id:string;
  Name:string;
  Description: string;
  Code: string;


  ngOnInit(): void {
    this.Id=this.proj.Id;
    this.Name=this.proj.Name;
    this.Description=this.proj.Description;
    this.Code=this.proj.Code;
  }


  addProject(){
    var val = { Id:this.Id,
                Name:this.Name,
                Description:this.Description,
                Code:this.Code};
    this.service.addProject(val).subscribe(res=>{
      alert("Project added");
    });
  }

  updateProject(){
    var val = { Id:this.Id,
                Name:this.Name,
                Description:this.Description,
                Code:this.Code};
    this.service.updateProject(val).subscribe(res=>{
    alert("Project updated");
    });
  }

}
