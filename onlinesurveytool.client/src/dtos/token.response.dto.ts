export class TokenResponseDTO {
  token: string;
  tokenType: string;

  constructor(token: string, tokenType: string) {
    this.token = token;
    this.tokenType = tokenType;
  }
}
