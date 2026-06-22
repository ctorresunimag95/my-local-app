import { test, expect } from '@playwright/test';

test.describe('Reservation Page', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the reservation page before each test
    await page.goto('/reservation');
  });

  test('should load the reservation page successfully', async ({ page }) => {
    // Check page title
    await expect(page).toHaveTitle('CinemaWeb');
    
    // Check page URL
    expect(page.url()).toContain('/reservation');
  });

  test('should display the header with logo and title', async ({ page }) => {
    // Check for header element (generic container with CTC Cinema text)
    const header = page.locator('text=CTC Cinema');
    await expect(header).toBeVisible();
  });

  test('should display navigation menu with all options', async ({ page }) => {
    // Check for Reservation link
    const reservationNav = page.locator('text=Reservation').first();
    await expect(reservationNav).toBeVisible();

    // Check for Management link
    const managementNav = page.locator('text=Management');
    await expect(managementNav).toBeVisible();

    // Check for Logout link
    const logoutNav = page.locator('text=Logout');
    await expect(logoutNav).toBeVisible();
  });

  test('should display the main content area', async ({ page }) => {
    // Verify there is a main content area visible by checking for movie containers
    const movieContainer = page.locator('div:has-text("Toy Story")').first();
    await expect(movieContainer).toBeVisible();
  });

  test('should have clickable navigation items', async ({ page }) => {
    // Check that Reservation link is clickable
    const reservationNav = page.locator('text=Reservation').first();
    await expect(reservationNav).toHaveCSS('cursor', 'pointer');

    // Check that Management link is clickable
    const managementNav = page.locator('text=Management');
    await expect(managementNav).toHaveCSS('cursor', 'pointer');
  });

  test('should respond to viewport resize', async ({ page }) => {
    // Test on desktop size
    await page.setViewportSize({ width: 1920, height: 1080 });
    await expect(page).toHaveTitle('CinemaWeb');

    // Test on mobile size
    await page.setViewportSize({ width: 375, height: 667 });
    await expect(page).toHaveTitle('CinemaWeb');
  });
});
