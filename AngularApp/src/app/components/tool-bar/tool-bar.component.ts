import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthProvider } from 'src/app/provider/auth.provider';

@Component({
  selector: 'app-tool-bar',
  templateUrl: './tool-bar.component.html',
  styleUrls: ['./tool-bar.component.css']
})
export class ToolBarComponent {
  constructor(private authProvider: AuthProvider, private router: Router) {}

  isAuthenticated = (): boolean => {
    return !this.authProvider.isAuthenticated();
  }

  logout = (): void => {
    this.authProvider.clearLocalStorage();
    this.router.navigate(['/']);
  }
}
