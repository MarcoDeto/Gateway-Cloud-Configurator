import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { ReplaySubject, Subject, Subscription } from 'rxjs';
import { debounceTime, delay, filter, map, takeUntil, tap } from 'rxjs/operators';
import { Entity, TypeEntity } from '../../Models/Entity.model';
import { Mode } from '../../Models/Mode.model';
import { EntityService } from '../../Services/entity.service';
import { typeEntityService } from '../../Services/typeEntity.service';
import { InterfaceGroupsComponent } from '../interface-groups/interface-groups.component';

@Component({
  selector: 'app-type-entity',
  templateUrl: './type-entity.component.html',
  styleUrls: ['./type-entity.component.scss']
})
export class TypeEntityComponent implements OnInit {
  title = "Tipologia Interfacce";
  subscribers: Subscription[] = [];
  mode: Mode = Mode.New;
  typologyEntity: TypeEntity | undefined = undefined;
  selectEntity: Entity[] = [];

  /** control for filter for server side. */
  public filterEntity: FormControl = new FormControl();
  /** indicate search operation is in progress */
  public searching = false;
  /** list of typeEntity filtered after simulating server side search */
  public filteredEntity: ReplaySubject<Entity[]> = new ReplaySubject<Entity[]>(1);
  /** Subject that emits when the component has been destroyed. */
  protected _onDestroy = new Subject<void>();

  typeEntityForm = this.formBuilder.group({
    id: ["", [Validators.required, Validators.maxLength(10)]],
    description: ["", [Validators.required, Validators.maxLength(80)]],
    entity: [this.data.entity, [Validators.required, Validators.maxLength(50)]],
    param1: ["", [Validators.maxLength(80)]],
    param2: ["", [Validators.maxLength(80)]],
    param3: ["", [Validators.maxLength(80)]],
    allowEditChannel: false,
    allowEditVariable: false
  });

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<InterfaceGroupsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private service: typeEntityService,
    private entityService: EntityService,
    private snackBar: MatSnackBar
  ) { }

  IsEditMode() { return this.mode === Mode.Edit; }
  IsNewMode() { return this.mode === Mode.New; }

  ngOnInit(): void {
    this.mode = this.data.mode;

    // filtra la lista in base al valore dell'input
    this.filterEntity.valueChanges
      .pipe(
        filter(search => !!search),
        tap(() => this.searching = true),
        takeUntil(this._onDestroy),
        debounceTime(200),
        map(search => {
          if (!this.selectEntity) {
            return [];
          }
          // simulate server fetching and filtering data
          return this.selectEntity.filter(x => x.description.indexOf(search) > -1);
        }),
        delay(200),
        takeUntil(this._onDestroy)
      )
      .subscribe(filtered => {
        this.searching = false;
        this.filteredEntity.next(filtered);
      },
        error => {
          this.searching = false;
        });
  }

  closeDialog() {
      this.dialogRef.close(null);
  }

  onSubmit() {
    if (this.IsNewMode()) {
      this.subscribers.push(this.service.Add(this.typeEntityForm.value).subscribe(
        res => {
          this.dialogRef.close(this.typeEntityForm.value);
          this.snackBar.open(res.message, "Done", {
            duration: 2000,
            panelClass: "success"
          });
        }
      ));
    }
    else {
      this.subscribers.push(this.service.Edit(this.typeEntityForm.value).subscribe(
        res => {
          this.dialogRef.close(this.typeEntityForm.value);
          this.snackBar.open(res.message, 'Done', {
            duration: 2000,
            panelClass: "success"
          });
        }
      ));
    }
  }
}
