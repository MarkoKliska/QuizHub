import React from 'react';

interface ButtonProps {
  onClick?: () => void;
  children: React.ReactNode;
  type?: 'button' | 'submit';
  className?: string;
}

const Button: React.FC<ButtonProps> = ({ onClick, children, type = 'button', className }) => {
  return (
    <button
      type={type}
      onClick={onClick}
      className={`bg-primary text-white px-4 py-2 rounded-md hover:bg-accent focus:outline-none focus:ring-2 focus:ring-tertiary ${className}`}
    >
      {children}
    </button>
  );
};

export default Button;