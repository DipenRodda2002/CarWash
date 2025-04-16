import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WasherFooterComponent } from './washer-footer.component';

describe('WasherFooterComponent', () => {
  let component: WasherFooterComponent;
  let fixture: ComponentFixture<WasherFooterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WasherFooterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WasherFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
