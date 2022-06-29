import { Component, OnInit } from '@angular/core';
import { Sample } from '../_models/sample';
import { Pagination } from '../_models/pagination';
import { ConfirmService } from '../_services/confirm.service';
import { SampleService } from '../_services/sample.service';

@Component({
  selector: 'app-samples',
  templateUrl: './samples.component.html',
  styleUrls: ['./samples.component.css']
})
export class SamplesComponent implements OnInit {
  samples: Sample[] = [];
  pagination: Pagination;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;
  loading = false;

  constructor(private sampleService: SampleService, private confirmService: ConfirmService) { }

  ngOnInit(): void {
    this.loadSamples();
  }

  loadSamples() {
    this.loading = true;
    this.sampleService.getSamples(this.pageNumber, this.pageSize, this.container).subscribe(response => {
      this.samples = response.result;
      this.pagination = response.pagination;
      this.loading = false;
    })
  }

  deleteSample(id: number) {
    this.confirmService.confirm('Confirm delete sample', 'This cannot be undone').subscribe(result => {
      if (result) {
        this.sampleService.deleteSample(id).subscribe(() => {
          this.samples.splice(this.samples.findIndex(m => m.id === id), 1);
        })
      }
    })

  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadSamples();
  }

}
