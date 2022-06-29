export interface Sample {
    id: number;
    senderId: number;
    senderUsername: string;
    senderPhotoUrl: string;
    product: string;
    productId: string;
    created?: Date;
    senderDeleted: boolean;
  }