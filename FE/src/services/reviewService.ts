import { API_BASE_URL } from '../config/apiConfig';
import { getAuthToken } from './authService';

export interface ReviewCreateDto {
  hotelId: number;
  bookingCode: string;
  rating: number;
  comment?: string;
}

export interface ReviewResponseDto {
  id: number;
  rating: number;
  comment?: string;
  createdAt: string;
  username: string;
}

class ReviewService {
  /**
   * Submit a new review after checkout
   */
  static async submitReview(reviewData: ReviewCreateDto): Promise<void> {
    const token = getAuthToken();
    if (!token) {
      throw new Error('Vui lòng đăng nhập để gửi đánh giá.');
    }

    const response = await fetch(`${API_BASE_URL}/api/reviews`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(reviewData)
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Gửi đánh giá thất bại');
    }
  }

  /**
   * Get reviews by hotel
   */
  static async getReviewsByHotel(hotelId: number): Promise<ReviewResponseDto[]> {
    const response = await fetch(`${API_BASE_URL}/api/reviews/hotel/${hotelId}`);

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Lỗi khi tải danh sách đánh giá');
    }

    const result = await response.json();
    return result.data;
  }
}

export default ReviewService;
