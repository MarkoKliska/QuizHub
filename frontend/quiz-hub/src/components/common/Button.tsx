import React from 'react';

interface ButtonProps {
  onClick?: () => void;
  children: React.ReactNode;
  type?: 'button' | 'submit';
  className?: string;
  disabled?: boolean;
}

const Button: React.FC<ButtonProps> = ({ onClick, children, type = 'button', className, disabled }) => {
  return (
    <button
      type={type}
      onClick={onClick}
      className={`bg-secondary text-white px-4 py-2 rounded-md hover:bg-accent focus:outline-none focus:ring-2 focus:ring-tertiary ${className}`}
      disabled={disabled}
    >
      {children}
    </button>
  );
};

export default Button;