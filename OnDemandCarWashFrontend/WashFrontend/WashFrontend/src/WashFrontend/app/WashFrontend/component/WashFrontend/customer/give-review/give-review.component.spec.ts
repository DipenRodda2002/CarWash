import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiveReviewComponent } from './give-review.component';

describe('GiveReviewComponent', () => {
  let component: GiveReviewComponent;
  let fixture: ComponentFixture<GiveReviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiveReviewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GiveReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
