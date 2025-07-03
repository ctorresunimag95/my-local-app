import { Routes } from "@angular/router";
import { PublishComponent } from "./movies/publish/publish.component";

export const MANAGEMENT_ROUTES : Routes = [
    {
        path: 'movies/publish',
        component: PublishComponent
    }
];