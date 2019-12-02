import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { DialogConfirmService } from './dialogconfirm.service';
import { Consulta } from '../consulta/consulta.model';
import { ConsultaService } from '../consulta/consulta.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-lista-consulta',
  templateUrl: './lista-consulta.component.html'
})

export class ListaConsultaComponent implements OnInit {
  listaConsultaForm: FormGroup;
  pesquisa: Pesquisa;
  consultas: Consulta[];
  datepipe: DatePipe;

  constructor(private dialogconfirmService: DialogConfirmService, private consultaService: ConsultaService) {
    this.listar();
  }

  ngOnInit() {
    this.listaConsultaForm = new FormGroup({
      "search": new FormControl()
    });
  }

  listar() {
    if (this.listaConsultaForm)
      this.pesquisa = Object.assign(this.listaConsultaForm.value);
    else
      this.pesquisa = { "search": "" };

    this.consultaService.getConsultas(this.pesquisa.search)
      .then(result => {
        result.forEach(c => {
          c.dataNascimento = new Date(c.dataNascimento).toLocaleDateString();
          c.dataInicial = new Date(c.dataInicial).toLocaleString();
          c.dataFinal = new Date(c.dataFinal).toLocaleString();
        });
        this.consultas = result;
      });
  }

  excluir(consulta: Consulta) {
    this.dialogconfirmService.confirm('Deseja excluir a consulta do paciente ' + consulta.nomePaciente + ' ?')
      .then((podeDeletar: boolean) => {
        if (podeDeletar) {
          this.consultaService.delete(consulta)
            .then(() => {
              this.consultas = this.consultas.filter((c: Consulta) => c.id != consulta.id);
            });
        }
      });
  }  
}

interface Pesquisa {
  search: string;
}
