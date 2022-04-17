import { User } from './user';
import { PaginationParams } from './paginationParams';

export class UserParams extends PaginationParams {
  gender: string;
  minAge = 18;
  maxAge = 99;
  pageNumber = 1;
  pageSize = 20;
  orderBy = 'lastActive';

  constructor(user: User) {
    super();
    this.gender = user.gender === 'female' ? 'male' : 'female';
  }
}
