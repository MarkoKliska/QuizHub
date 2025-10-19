export interface QuizAttempt {
  id: string;
  userId: string;
  quizId: string;
  quizName: string;
  startTime: string;
  endTime: string | null;
  score: number;
  percentage: number;
}

export interface Answer {
  questionId: string;
  selectedOptionIds?: string[];
  textAnswer?: string;
}

export interface QuizAttemptDetails {
  quizName: string;
  score: number;
  percentage: number;
  startTime: string;
  endTime: string;
  questions: QuestionDetail[];
}

export interface QuestionDetail {
  text: string;
  type: string;
  options: OptionDetail[];
  isCorrect: boolean;
  userAnswer?: string;
  correctAnswer?: string;
}

export interface OptionDetail {
  text: string;
  isCorrect: boolean;
  selected: boolean;
}