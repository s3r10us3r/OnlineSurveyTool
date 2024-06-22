export class LoginResponse {
  accessToken: string;
  accessExpirationDateTime: Date;
  refreshToken: string;
  refreshExpirationDateTime: Date;

  constructor(accessToken: string, accessExpiration: string , refreshToken: string, refreshTokenExpiration: string ) {
    this.accessToken = accessToken;
    this.accessExpirationDateTime = new Date(accessExpiration);
    this.refreshToken = refreshToken;
    this.refreshExpirationDateTime = new Date(refreshTokenExpiration);
  }
}
