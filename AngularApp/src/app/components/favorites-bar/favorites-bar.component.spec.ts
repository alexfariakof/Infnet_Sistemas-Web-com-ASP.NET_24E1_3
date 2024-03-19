import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FavoritesBarComponent } from './favorites-bar.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { PlaylistManagerService } from '../../services';

describe('FavoritesBarComponent', () => {
  let component: FavoritesBarComponent;
  let fixture: ComponentFixture<FavoritesBarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FavoritesBarComponent],
      imports: [HttpClientTestingModule],
      providers: [PlaylistManagerService]
    });
    fixture = TestBed.createComponent(FavoritesBarComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

});
