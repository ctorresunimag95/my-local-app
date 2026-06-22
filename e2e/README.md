# Cinema Web E2E Tests

End-to-end tests for the Cinema Web Application using Playwright.

## Setup

### Prerequisites
- Node.js 16 or higher
- npm or yarn

### Installation

```bash
cd e2e
npm install
```

## Running Tests

### Run all tests
```bash
npm test
```

### Run tests in headed mode (see the browser)
```bash
npm run test:headed
```

### Run tests in debug mode
```bash
npm run test:debug
```

### Run tests with UI (interactive)
```bash
npm run test:ui
```

### View test reports
```bash
npm run test:report
```

## Test Suites

### 1. **Reservation Page** (`reservation.spec.ts`)
Tests the basic functionality and layout of the reservation page:
- Page loads successfully with correct title
- Header with logo is displayed
- Navigation menu with all options (Reservation, Management, Logout)
- Main content area is visible
- Navigation items are clickable
- Page responds to viewport resize

### 2. **Movies Loading** (`movies.spec.ts`)
Tests that movies are properly loaded with images and titles:
- Exactly 3 movies are loaded
- All movies display with images
- Toy Story movie displays with image and title
- xXx movie displays with image and title
- Titanic movie displays with image and title
- Movie images have valid src attributes
- Movies display in correct order
- Movie containers are clickable
- All movie images load correctly
- All movies display in a container

### 3. **Movie Interactions** (`movies.interactions.spec.ts`)
Tests user interactions with movies:
- Clicking on Toy Story movie
- Clicking on xXx movie
- Clicking on Titanic movie
- Navigating via Management link
- Movies display with proper spacing
- Movie images display with correct dimensions
- Movie elements are accessible
- Layout persists on page refresh

## Configuration

The test configuration is defined in `playwright.config.ts`:
- **Base URL**: http://localhost:54163
- **Browser**: Chromium, Firefox, WebKit
- **Mobile**: Pixel 5, iPhone 12
- **Reporter**: HTML

## Recorded Elements

The page structure has the following key elements:
- `e3`: Main container
- `e4`: Header area
- `e5`: Logo/Brand area
- `e9`: "CTC Cinema" text
- `e10-e13`: Navigation items (Reservation, Management, Logout)
- `e20`: Movies container
- `e22, e30, e38`: Movie containers (clickable)
- `e24, e32, e40`: Movie images
- `e28, e36, e44`: Movie titles (Toy Story, xXx, Titanic)

## Troubleshooting

### Tests timeout
Make sure the Cinema Web application is running on http://localhost:54163

### Browser not found
Run `npx playwright install` to install the browsers

### Port already in use
Update the `baseURL` in `playwright.config.ts` to the correct port

## CI/CD Integration

To run tests in CI/CD pipeline:
```bash
CI=true npm test
```

This will:
- Run tests with retries (2 retries on failure)
- Run in serial (not parallel)
- Generate HTML reports

## Continuous Monitoring

To watch tests while developing:
```bash
npx playwright test --watch
```

## Debugging

### Visual debugging
```bash
npm run test:debug
```

### Trace debugging
Check the `trace.zip` file in the test results for detailed event replay.

### Screenshots on failure
Screenshots are automatically captured on test failure in the results directory.
