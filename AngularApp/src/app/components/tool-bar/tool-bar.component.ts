import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthProvider } from '../../provider/auth.provider';
import { MusicService } from '../../services';

@Component({
  selector: 'app-tool-bar',
  templateUrl: './tool-bar.component.html',
  styleUrls: ['./tool-bar.component.css']
})
export class ToolBarComponent {

  constructor(private authProvider: AuthProvider,
    private router: Router,
    public musicService: MusicService) {}

  isAuthenticated = (): boolean => {
    return !this.authProvider.isAuthenticated();
  }

  logout = (): void => {
    this.authProvider.clearSessionStorage();
    this.router.navigate(['/']);
  }


}
