import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-plot-type',
  standalone: false,
  templateUrl: './plot-type.component.html',
  styleUrl: './plot-type.component.scss'
})
export class PlotTypeComponent implements OnInit {
  plotTypes: { number: number; name: string }[] = [];

   ngOnInit(): void {
    this.plotTypes = [
      { number: 1, name: 'Residential' },
      { number: 2, name: 'Commercial' },
      { number: 3, name: 'Industrial' },
      { number: 4, name: 'Farm House' },
      { number: 5, name: 'Park Facing' },
      { number: 6, name: 'Corner' },
      { number: 7, name: 'Main Boulevard' },
      { number: 8, name: 'Facing School' },
      { number: 9, name: 'Facing Masjid' },
      { number: 10, name: 'Facing Playground' },
      { number: 11, name: 'Facing Market' },
      { number: 12, name: 'Overseas' },
      { number: 13, name: 'Low Cost' },
      { number: 14, name: 'High Rise' },
      { number: 15, name: 'Mixed Use' },
      { number: 16, name: 'Apartment' },
      { number: 17, name: 'Utility' },
      { number: 18, name: 'Public Building' },
      { number: 19, name: 'Green Belt Facing' },
      { number: 20, name: 'West Open' },
      { number: 21, name: 'Other' },
    ];
}}