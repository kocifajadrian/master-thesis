use egui::{Context, Slider, TextEdit, Checkbox, RichText, TextStyle};

pub fn gui_settings(ui: &Context) {
    egui::Window::new(RichText::new("Visualization Settings").size(24.0))
        .default_open(true)
        .max_width(500.0)
        .max_height(500.0)
        .default_width(800.0)
        .resizable(false)
        .show(ui, |ui| {
            let mut slice_dim = 3;
            ui.horizontal(|ui| {
                ui.label(RichText::new("Dimensions:").size(18.0));
                ui.add(
                    Slider::new(&mut slice_dim, 4..=6)
                        .text(RichText::new("").size(16.0))
                );
            });

            ui.separator();

            let mut opacity = 0.75;
            ui.horizontal(|ui| {
                ui.label(RichText::new("Opacity:").size(18.0));
                ui.add(
                    Slider::new(&mut opacity, 0.0..=1.0)
                        .text(RichText::new("").size(16.0))
                );
            });

            ui.separator();

            let mut show_bbox = true;
            ui.checkbox(&mut show_bbox, RichText::new("Show Bounding Box").size(18.0));

            ui.separator();

            let mut expr = "".to_string();
            ui.horizontal(|ui| {
                ui.label(RichText::new("Function").size(18.0));
                ui.add(
                    TextEdit::singleline(&mut expr)
                        .font(TextStyle::Heading)
                        .desired_width(250.0)
                );
            });

            ui.separator();

            ui.horizontal(|ui| {
                if ui.button(RichText::new("▶ Render Volume").size(18.0)).clicked() {}
                if ui.button(RichText::new("↻ Reset View").size(18.0)).clicked() {}
            });
        });
}
