import { ModuleWithProviders, NgModule } from '@angular/core';
import { NepaliDatepickerComponent } from './nepali-datepicker-angular.component';
import { ToNpPipe } from './pipes/to-np.pipe';
import { CommonModule, DatePipe } from '@angular/common';
import { ConfigType } from './interface/interface';
import { FormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@NgModule({
  declarations: [ToNpPipe, NepaliDatepickerComponent],
  imports: [CommonModule,FormsModule, CalendarModule,
    BsDatepickerModule.forRoot()],
  providers: [DatePipe],
  exports: [NepaliDatepickerComponent],
})
export class NepaliDatepickerModule {
  static forRoot(
    config: ConfigType
  ): ModuleWithProviders<NepaliDatepickerModule> {
    return {
      ngModule: NepaliDatepickerModule,
      providers: [
        {
          provide: 'config',
          useValue: config,
        },
      ],
    };
  }
}
