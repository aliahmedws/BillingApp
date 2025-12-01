import { CoreModule } from '@abp/ng.core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgModule } from '@angular/core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzStepsModule } from 'ng-zorro-antd/steps';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { UploadDropzoneComponent } from '../components/upload-dropzone/upload-dropzone.component';
import { NzTabsModule } from 'ng-zorro-antd/tabs';

@NgModule({
  declarations: [],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    NzSelectModule,
    NzTableModule,
    NzStepsModule,
    NzUploadModule,
    NzIconModule,
    UploadDropzoneComponent,
    NzTabsModule
  ],
  exports: [
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    NzSelectModule,
    NzTableModule,
    NzStepsModule,
    NzUploadModule,
    NzIconModule,
    UploadDropzoneComponent,
    NzTabsModule
  ],
  providers: []
})
export class SharedModule {}
