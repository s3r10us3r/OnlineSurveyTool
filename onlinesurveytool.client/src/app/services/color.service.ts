import {Injectable} from "@angular/core";

@Injectable()
export class ColorService{
  public assignColors(quantity: number): string[]{
    const res = [];
    for(let i = 0; i < quantity; i++)
      res.push(this.getRandomColorString())
    return res;
  }

  private getRandomColorString(): string {
    const hue = Math.floor(Math.random() * 360);
    const saturation = (70 + Math.random() * 30) / 100;
    const lightness = (50 + Math.random() * 10) / 100;

    const rgb = this.hsl2rgb(hue, saturation, lightness);
    return '#' + rgb.r.toString(16) + rgb.g.toString(16) + rgb.b.toString(16);
  }

  private hsl2rgb(h: number, s: number, l: number): {r: number, g: number, b:number} {
    const c = (1 - Math.abs(2*l - 1)) * s;
    const hp = h / 60;
    const x = c * (1 - Math.abs(hp % 2 - 1))
    const rgb1 = this.hpSwitch(hp, x, c);
    const m = l - c/2;
    return {
      r: Math.floor((rgb1[0] + m) * 255),
      g: Math.floor((rgb1[1] + m) * 255),
      b: Math.floor((rgb1[2] + m) * 255)
    }
  }

  private hpSwitch(hp: number, x: number, c: number) : [number, number, number]{
    if (hp <= 1)
      return [c, x, 0];
    if (hp <= 2)
      return [x, c, 0];
    if (hp <= 3)
      return [0, c, x];
    if (hp <= 4)
      return [0, x, c];
    if (hp <= 5)
      return [x, 0, c];

    return [c, 0, x];
  }}
