import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProjectsComponent } from './show-projects.component';

describe('ShowProjectsComponent', () => {
  let component: ShowProjectsComponent;
  let fixture: ComponentFixture<ShowProjectsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowProjectsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
