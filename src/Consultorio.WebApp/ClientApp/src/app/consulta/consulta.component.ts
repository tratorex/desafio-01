import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Consulta } from './consulta.model';
import { ConsultaService } from './consulta.service';
import { Location } from '@angular/common';
import { ActivatedRoute, Params } from '@angular/router';


@Component({
  selector: 'app-consulta',
  templateUrl: './consulta.component.html',
  styleUrls: ['./consulta.component.css']
})

export class ConsultaComponent implements OnInit {
  consultaForm: FormGroup;
  consultaModel: Consulta;
  constructor(private route: ActivatedRoute, private location : Location, private consultaService : ConsultaService) { }

  ngOnInit() {
    this.consultaForm = new FormGroup({
      "id": new FormControl(),
      "nomePaciente": new FormControl(null, [Validators.required, Validators.maxLength(100)]),
      "dataNascimento": new FormControl(null, [Validators.required]),
      "dataInicial": new FormControl(null, [Validators.required]),
      "dataFinal": new FormControl(null, [Validators.required]),
      "observacoes": new FormControl(null, [Validators.maxLength(300)]),
      "errorMessage": new FormControl()
    });

    this.route.params.forEach((params: Params) => {
      let id: number = +params['id'];
      if (id) {
        this.consultaService.getConsulta(id)
          .then((consulta: Consulta) => {
            consulta.dataNascimento = consulta.dataNascimento.substring(0, 10);
            this.consultaForm.setValue(consulta);
          });
      }
    });
  }

  get datasInvalidas() {
    this.consultaModel = Object.assign(this.consultaForm.value);
    return (new Date(this.consultaModel.dataFinal) < new Date(this.consultaModel.dataInicial));
  }

  get temErro() {
    return this.consultaForm.get("errorMessage").value;
  }

  agendar() {
    this.consultaModel = Object.assign(this.consultaForm.value);

    let promise;
    if (this.consultaModel.id == "0" || this.consultaModel.id == undefined) {
        this.consultaModel.id = "0";
        promise = this.consultaService.create(this.consultaModel);
    } else {
        promise = this.consultaService.update(this.consultaModel);
    }
    promise.then(consulta => this.location.back());
    promise.catch(err => this.consultaForm.get("errorMessage").setValue(err));
  }
}
