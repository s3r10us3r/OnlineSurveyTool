export class UserRegisterDTO {
  login: string | null | undefined;
  email: string | null | undefined;
  password: string | null | undefined;

  constructor(login: string | null | undefined, email: string | null | undefined, password: string | null | undefined) {
    this.login = login;
    this.email = email;
    this.password = password;
  }
}
