import { render, screen } from '@testing-library/react';
import Home from './Home';
import { BrowserRouter } from 'react-router-dom';

test('renders by something link', () => {
  render(<Home />, { wrapper: BrowserRouter });
  const linkElement = screen.getByText(/buy something/i);
  expect(linkElement).toBeInTheDocument();
});
