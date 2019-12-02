import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Consulta } from './consulta.model';

@Injectable()
export class ConsultaService {
  constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) { }
  private consultaUrl: string = this.baseUrl + 'api/Consulta';

  getConsultas(filtro: string): Promise<Consulta[]> {
    return this.http.get(this.consultaUrl + (filtro != '' ? '?Search=' + filtro : ''))
      .toPromise()
      .then(response => response.json().data as Consulta[])
      .catch(this.trataErro);
  }

  private trataErro(err: any): Promise<any> {
    return Promise.reject(JSON.parse(err._body).message[0].values[0] || err);
  }

  getConsulta(id: number): Promise<Consulta> {
    return this.http.get(this.consultaUrl + '/' + id)
      .toPromise()
      .then(response => response.json() as Consulta)
      .catch(this.trataErro);
  }

  create(consulta: Consulta): Promise<Consulta> {
    return this.http
      .post(this.consultaUrl, consulta)
      .toPromise()
      .then((response: Response) => response.json().data as Consulta)
      .catch(this.trataErro);
  }

  update(consulta: Consulta): Promise<Consulta> {
    return this.http
      .put(this.consultaUrl, consulta)
      .toPromise()
      .then(() => consulta as Consulta)
      .catch(this.trataErro);
  }

  delete(consulta: Consulta) {
    const url = `${this.consultaUrl}/${consulta.id}`;
    return this.http
      .delete(url)
      .toPromise()
      .then((response) => response)
      .catch(this.trataErro);
  }
}
