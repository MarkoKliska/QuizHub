import React from 'react';

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  className?: string;
}

const Input: React.FC<InputProps> = ({ className = '', ...props }) => {
  return (
    <input
      {...props}
      className={`p-3 border border-accent rounded-md focus:outline-none focus:ring-2 focus:ring-secondary ${className}`}
    />
  );
};

export default Input;