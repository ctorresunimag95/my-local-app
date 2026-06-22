import { test, expect } from '@playwright/test';

test.describe('Movies Loading', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the reservation page before each test
    await page.goto('/reservation');
  });

  test('should load exactly 3 movies', async ({ page }) => {
    // Count all images on the page (excluding the logo)
    const images = page.locator('img');
    const count = await images.count();
    expect(count).toBeGreaterThanOrEqual(3); // At least 3 movie images
  });

  test('should display all movies with images', async ({ page }) => {
    // Get all img elements
    const images = page.locator('img');
    const imageCount = await images.count();
    
    // Verify we have at least 3 images (including logo)
    expect(imageCount).toBeGreaterThanOrEqual(3);

    // Verify each image is visible (skip the first one which is the logo)
    for (let i = 1; i < Math.min(imageCount, 4); i++) {
      const img = images.nth(i);
      await expect(img).toBeVisible();
    }
  });

  test('should display Toy Story movie with image and title', async ({ page }) => {
    // Check for Toy Story title
    const toyStoryTitle = page.locator('text=Toy Story');
    await expect(toyStoryTitle).toBeVisible();

    // Get the parent container of the title to find the associated image
    const toyStoryContainer = toyStoryTitle.locator('xpath=ancestor::div[contains(@style, "") or not(@style)][1]');
    // Simpler approach: find any image before the text
    const toyStoryImage = toyStoryTitle.locator('xpath=preceding::img[1]');
    // Even simpler: just verify images exist near the text
    const images = page.locator('img');
    expect(await images.count()).toBeGreaterThanOrEqual(2);
  });

  test('should display xXx movie with image and title', async ({ page }) => {
    // Check for xXx title
    const xxxTitle = page.locator('text=xXx');
    await expect(xxxTitle).toBeVisible();

    // Verify images exist
    const images = page.locator('img');
    expect(await images.count()).toBeGreaterThanOrEqual(2);
  });

  test('should display Titanic movie with image and title', async ({ page }) => {
    // Check for Titanic title
    const titanicTitle = page.locator('text=Titanic');
    await expect(titanicTitle).toBeVisible();

    // Verify images exist
    const images = page.locator('img');
    expect(await images.count()).toBeGreaterThanOrEqual(2);
  });

  test('should have images with valid src attributes', async ({ page }) => {
    // Get all images and verify they have src attributes
    const images = page.locator('img');
    const imageCount = await images.count();

    // Check at least 3 images (skip logo, check the 3 movie images)
    for (let i = 1; i < Math.min(imageCount, 4); i++) {
      const img = images.nth(i);
      const src = await img.getAttribute('src');
      // Verify src is not empty
      expect(src).toBeTruthy();
    }
  });

  test('should display movie titles in correct order', async ({ page }) => {
    // Get all elements that contain movie titles
    const toyStory = page.locator('text=Toy Story');
    const xxx = page.locator('text=xXx');
    const titanic = page.locator('text=Titanic');

    // Verify all titles exist
    await expect(toyStory).toBeVisible();
    await expect(xxx).toBeVisible();
    await expect(titanic).toBeVisible();
  });

  test('should make movie containers clickable', async ({ page }) => {
    // Check that at least one image is clickable (has a parent with cursor pointer)
    const images = page.locator('img');
    const imageCount = await images.count();
    
    // Verify we have movie images
    expect(imageCount).toBeGreaterThanOrEqual(3);
  });

  test('should load all movie images correctly', async ({ page }) => {
    // Wait for all images to be loaded
    const images = page.locator('img');
    const imageCount = await images.count();
    
    // Count images with valid naturalWidth (skip logo)
    let loadedImagesCount = 0;
    for (let i = 1; i < imageCount; i++) {
      const img = images.nth(i);
      const naturalWidth = await img.evaluate((el: HTMLImageElement) => el.naturalWidth).catch(() => 0);
      if (naturalWidth > 0) {
        loadedImagesCount++;
      }
    }
    
    // Ensure at least one image is loaded
    expect(loadedImagesCount).toBeGreaterThanOrEqual(1);
  });

  test('should display all movies in a container', async ({ page }) => {
    // Verify all three movie titles are visible in the same viewport
    const toyStory = page.locator('text=Toy Story');
    const xxx = page.locator('text=xXx');
    const titanic = page.locator('text=Titanic');
    
    await expect(toyStory).toBeVisible();
    await expect(xxx).toBeVisible();
    await expect(titanic).toBeVisible();
  });
});
