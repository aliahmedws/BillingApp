import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-consumer-document',
  standalone: false,
  templateUrl: './consumer-document.component.html',
  styleUrl: './consumer-document.component.scss'
})
export class ConsumerDocumentComponent {
  showFilter = false;

  constructor(
    private router: Router
  ) {}
  
  navigateToCreate() {
    this.router.navigate(['/createConsumerDocuments']);
  }
}
