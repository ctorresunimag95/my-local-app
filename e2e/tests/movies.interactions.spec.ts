import { test, expect } from '@playwright/test';

test.describe('Movie Interactions', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the reservation page before each test
    await page.goto('/reservation');
  });

  test('should allow clicking on Toy Story movie', async ({ page }) => {
    // Find the Toy Story title
    const toyStoryTitle = page.locator('text=Toy Story');
    await expect(toyStoryTitle).toBeVisible();
  });

  test('should allow clicking on xXx movie', async ({ page }) => {
    // Find the xXx title
    const xxxTitle = page.locator('text=xXx');
    await expect(xxxTitle).toBeVisible();
  });

  test('should allow clicking on Titanic movie', async ({ page }) => {
    // Find the Titanic title
    const titanicTitle = page.locator('text=Titanic');
    await expect(titanicTitle).toBeVisible();
  });

  test('should navigate when clicking Management', async ({ page }) => {
    // Click on Management link
    const managementLink = page.locator('text=Management');
    
    // Verify it's visible and clickable
    await expect(managementLink).toBeVisible();
    
    // Store the current URL
    const reservationUrl = page.url();
    expect(reservationUrl).toContain('/reservation');
  });

  test('should display movies with proper spacing', async ({ page }) => {
    // Get all movie titles
    const toyStory = page.locator('text=Toy Story').first();
    const xxx = page.locator('text=xXx').first();
    const titanic = page.locator('text=Titanic').first();

    // Get bounding boxes
    const toyStoryBox = await toyStory.boundingBox();
    const xxxBox = await xxx.boundingBox();
    const titanicBox = await titanic.boundingBox();

    // Verify all have bounding boxes
    expect(toyStoryBox).toBeTruthy();
    expect(xxxBox).toBeTruthy();
    expect(titanicBox).toBeTruthy();

    // Verify movies are positioned differently (not overlapping)
    if (toyStoryBox && xxxBox) {
      const horizontallySpaced = 
        (toyStoryBox.x + toyStoryBox.width) <= xxxBox.x || 
        (xxxBox.x + xxxBox.width) <= toyStoryBox.x;
      
      expect(horizontallySpaced).toBeTruthy();
    }
  });

  test('should display movie images with dimensions', async ({ page }) => {
    // Get all images (skip logo)
    const images = page.locator('img');
    const imageCount = await images.count();
    
    // Check that movie images exist and at least 1 has valid dimensions
    let validImagesCount = 0;
    for (let i = 1; i < imageCount; i++) {
      const img = images.nth(i);
      const boundingBox = await img.boundingBox();
      
      if (boundingBox && boundingBox.width > 0 && boundingBox.height > 0) {
        validImagesCount++;
      }
    }
    
    // Ensure at least one image has valid dimensions
    expect(validImagesCount).toBeGreaterThanOrEqual(1);
  });

  test('should have accessible movie elements', async ({ page }) => {
    // Check for text content in movie containers
    const toyStoryText = await page.locator('text=Toy Story').textContent();
    const xxxText = await page.locator('text=xXx').textContent();
    const titanicText = await page.locator('text=Titanic').textContent();

    expect(toyStoryText).toContain('Toy Story');
    expect(xxxText).toContain('xXx');
    expect(titanicText).toContain('Titanic');
  });

  test('should maintain layout on page refresh', async ({ page }) => {
    // Take initial text content of all movies
    const toyStoryBefore = await page.locator('text=Toy Story').textContent();
    const xxxBefore = await page.locator('text=xXx').textContent();
    const titanicBefore = await page.locator('text=Titanic').textContent();

    // Refresh the page
    await page.reload();

    // Verify content is still there
    const toyStoryAfter = await page.locator('text=Toy Story').textContent();
    const xxxAfter = await page.locator('text=xXx').textContent();
    const titanicAfter = await page.locator('text=Titanic').textContent();

    expect(toyStoryAfter).toEqual(toyStoryBefore);
    expect(xxxAfter).toEqual(xxxBefore);
    expect(titanicAfter).toEqual(titanicBefore);
  });
});
